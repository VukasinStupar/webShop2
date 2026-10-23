
import { useEffect, useState } from "react";
import Header from "./components/Header";
import Hero from "./components/Hero";
import Marketplace from "./components/Marketplace";
import CategoryStats from "./components/CategoryStats";
import Footer from "./components/Footer";
import "./App.css";
import apiUrl from "./api";
import CreateProduct from "./components/CreateProduct";

function App() {
    const [search, setSearch] = useState("");
    const [selectedCategory, setSelectedCategory] = useState("Sve kategorije");
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);
    const [showCreateProduct, setShowCreateProduct] = useState(false);
    const [productsRefreshKey, setProductsRefreshKey] = useState(0);
    const [sortBy, setSortBy] = useState("recommended");

    useEffect(() => {
        async function getCategories() {
            try {
                const response = await fetch(`${apiUrl}/api/Category`);

                if (!response.ok) {
                    throw new Error(
                        `Greška pri učitavanju kategorija: ${response.status}`
                    );
                }

                const data = await response.json();
                setCategories(data);
            } catch (error) {
                console.error("Greška:", error);
            }
        }

        getCategories();
    }, []);

    useEffect(() => {
        async function getProducts() {
            const endpoints = ["/api/Product", "/api/Products"];

            for (const endpoint of endpoints) {
                try {
                    const response = await fetch(`${apiUrl}${endpoint}`);

                    if (!response.ok) {
                        console.warn(
                            `Proizvodi: ${endpoint} vratio ${response.status}`
                        );
                        continue;
                    }

                    const data = await response.json();
                    console.log(`PROIZVODI (${endpoint}):`, data);

                    setProducts(
                        Array.isArray(data)
                            ? data
                            : data.items ?? data.products ?? []
                    );

                    return;
                } catch (error) {
                    console.error(`Greška (${endpoint}):`, error);
                }
            }
        }

        getProducts();
    }, [productsRefreshKey]);

    const stats = categories.map((cat) => ({
        name: cat.name,
        count: products
            .filter((p) => Number(p.categoryId) === Number(cat.id))
            .reduce((sum, p) => sum + (Number(p.quantity) || 0), 0),
    }));

    return (
        <div className="app">
            <Header
                search={search}
                onSearchChange={setSearch}
                onAddProduct={() => setShowCreateProduct(true)}
                categories={categories}
                selectedCategory={selectedCategory}
                onCategoryChange={setSelectedCategory}
                sortBy={sortBy}
                onSortChange={setSortBy}
            />

            <main className="app__main">
                {/* <Hero /> */}

                <Marketplace
                    search={search}
                    categories={categories}
                    selectedCategory={selectedCategory}
                    onCategoryChange={setSelectedCategory}
                    sortBy={sortBy}
                />
            </main>

            <CategoryStats stats={stats} />

            {showCreateProduct && (
                <CreateProduct
                    categories={categories}
                    onCreated={() => {
                        setShowCreateProduct(false);
                        setProductsRefreshKey(
                            (previousKey) => previousKey + 1
                        );
                    }}
                    onClose={() => setShowCreateProduct(false)}
                />
            )}

            <Footer />
        </div>
    );
}

export default App;

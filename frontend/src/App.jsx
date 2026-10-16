
import { useEffect, useState } from "react";
import Header from "./components/Header";
import Hero from "./components/Hero";
import CategoryList from "./components/CategoryList";
import Marketplace from "./components/Marketplace";
import Footer from "./components/Footer";
import "./App.css";

function App() {
    const [search, setSearch] = useState("");
    const [selectedCategory, setSelectedCategory] = useState("Sve kategorije");
    const [categories, setCategories] = useState([]);

    useEffect(() => {
        async function getCategories() {
            try {
                // const response = await fetch(
                //     "https://localhost:51540/api/Category"
                // );

                const apiUrl = import.meta.env.VITE_API_URL;

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

    return (
        <div className="app">
            <Header
                search={search}
                onSearchChange={setSearch}
            />

            <main className="app__main">
                {/* <Hero /> */}

                <CategoryList
                    categories={categories}
                    selectedCategory={selectedCategory}
                    onCategoryChange={setSelectedCategory}
                />

                <Marketplace
    search={search}
    categories={categories}
    selectedCategory={selectedCategory}
    onCategoryChange={setSelectedCategory}
/>
            </main>

            <Footer />
        </div>
    );
}

export default App;

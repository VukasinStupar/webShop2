
// import { useEffect, useState } from "react";
// import Header from "./components/Header";
// import Hero from "./components/Hero";
// import CategoryList from "./components/CategoryList";
// import Marketplace from "./components/Marketplace";
// import Footer from "./components/Footer";
// import "./App.css";

// function App() {
//     const [search, setSearch] = useState("");
//     const [selectedCategory, setSelectedCategory] = useState("Sve kategorije");
//     const [categories, setCategories] = useState([]);

//     useEffect(() => {
//         async function getCategories() {
//             try {
//                 // const response = await fetch(
//                 //     "https://localhost:51540/api/Category"
//                 // );

//                 const apiUrl =
//     import.meta.env.VITE_API_URL || "https://localhost:51540";

// console.log("API adresa:", apiUrl);
// const response = await fetch(`${apiUrl}/api/Category`);

//                 if (!response.ok) {
//                     throw new Error(
//                         `Greška pri učitavanju kategorija: ${response.status}`
//                     );
//                 }

//                 const data = await response.json();
//                 setCategories(data);
//             } catch (error) {
//                 console.error("Greška:", error);
//             }
//         }

//         getCategories();
//     }, []);

//     return (
//         <div className="app">
//             <Header
//                 search={search}
//                 onSearchChange={setSearch}
//             />

//             <main className="app__main">
//                 {/* <Hero /> */}

//                 <CategoryList
//                     categories={categories}
//                     selectedCategory={selectedCategory}
//                     onCategoryChange={setSelectedCategory}
//                 />

//                 <Marketplace
//     search={search}
//     categories={categories}
//     selectedCategory={selectedCategory}
//     onCategoryChange={setSelectedCategory}
// />
//             </main>

//             <Footer />
//         </div>
//     );
// }

// export default App;

import { useEffect, useState } from "react";
import Header from "./components/Header";
import Hero from "./components/Hero";
import CategoryList from "./components/CategoryList";
import Marketplace from "./components/Marketplace";
import CategoryStats from "./components/CategoryStats"; // DODATO
import Footer from "./components/Footer";
import "./App.css";

function App() {
    const [search, setSearch] = useState("");
    const [selectedCategory, setSelectedCategory] = useState("Sve kategorije");
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]); // DODATO

    useEffect(() => {
        async function getCategories() {
            try {
                // const response = await fetch(
                //     "https://localhost:51540/api/Category"
                // );

                const apiUrl =
    import.meta.env.VITE_API_URL || "https://localhost:51540";

console.log("API adresa:", apiUrl);
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

    // DODATO: učitavanje proizvoda za statistiku
    useEffect(() => {
        async function getProducts() {
            const apiUrl =
                import.meta.env.VITE_API_URL || "https://localhost:51540";

            // probamo više mogućih naziva endpointa
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
                    console.log(`PROIZVODI (${endpoint}):`, data); // privremeno, za proveru

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
    }, []);

    // DODATO: broj proizvoda po kategoriji
    const stats = categories.map((cat) => ({
        name: cat.name,
        count: products.filter(
            (p) =>
                p.categoryId === cat.id ||
                p.category === cat.name ||
                p.categoryName === cat.name ||
                p.category?.id === cat.id ||
                p.category?.name === cat.name
        ).length,
    }));

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

            {/* DODATO: statistika iznad footera */}
            <CategoryStats stats={stats} />

            <Footer />
        </div>
    );
}

export default App;
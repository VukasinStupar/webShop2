
import { useEffect, useState } from "react";
import EmptyProducts from "./EmptyProducts";
import "./ProductList.css";

function ProductList({
    search,
    selectedCategory,
    categories = [],
    sortBy = "recommended"
}) {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        async function loadProducts() {
            try {
                setLoading(true);
                setError("");

                const response = await fetch(
                    "https://localhost:51540/api/Product"
                );

                if (!response.ok) {
                    throw new Error("Neuspešno učitavanje proizvoda.");
                }

                const data = await response.json();
                setProducts(data);
            } catch (error) {
                setError(
                    "Ne mogu da učitam proizvode. Proveri da li backend radi i da li je adresa API-ja ispravna."
                );
            } finally {
                setLoading(false);
            }
        }

        loadProducts();
    }, []);

    const normalizedSearch = (search ?? "").trim().toLocaleLowerCase("sr");

    const selectedCategoryId = categories.find(
        (category) => category.name === selectedCategory
    )?.id;

    const filteredProducts = products.filter((product) => {
        const productName = (product.name ?? "").toLocaleLowerCase("sr");
        const productDescription = (product.description ?? "").toLocaleLowerCase("sr");

        const matchesSearch =
            productName.includes(normalizedSearch) ||
            productDescription.includes(normalizedSearch);

        const matchesCategory =
            !selectedCategory ||
            selectedCategory === "Sve kategorije" ||
            Number(product.categoryId) === Number(selectedCategoryId);

        return matchesSearch && matchesCategory;
    });

    const sortedProducts = [...filteredProducts].sort((a, b) => {
        switch (sortBy) {
            case "price-asc":
                return Number(a.price) - Number(b.price);

            case "price-desc":
                return Number(b.price) - Number(a.price);

            case "newest":
                return Number(b.id) - Number(a.id);

            case "recommended":
            default:
                return 0;
        }
    });

    return (
        <div className="product-list">
            <div className="product-list__toolbar">
                <div className="product-list__result">
                    <span className="product-list__result-label">
                        Trenutna ponuda
                    </span>

                    <span className="product-list__result-value">
                        {filteredProducts.length} proizvoda
                    </span>
                </div>

                {search.trim() && (
                    <div className="product-list__search">
                        Pretraga: <strong>{search.trim()}</strong>
                    </div>
                )}
            </div>

            <div className="product-list__content">
                {loading ? (
                    <p>Učitavanje proizvoda...</p>
                ) : error ? (
                    <p>{error}</p>
                ) : sortedProducts.length === 0 ? (
                    <EmptyProducts />
                ) : (
                    <div className="product-grid">
                        {sortedProducts.map((product) => (
                            <article
                                className="product-card"
                                key={product.id}
                            >
                                <h3>{product.name}</h3>

                                <p>{product.description}</p>

                                <p className="product-card__price">
                                    {product.price.toLocaleString("sr-RS")} RSD
                                </p>

                                <p>
                                    Dostupna količina: {product.quantity}
                                </p>

                                <p>Status: {product.status}</p>
                            </article>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}

export default ProductList;

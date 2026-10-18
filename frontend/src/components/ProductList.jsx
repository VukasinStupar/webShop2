import { useCallback, useEffect, useRef, useState } from "react";
import EmptyProducts from "./EmptyProducts";
import ProductProfile from "./ProductProfile";
import { getMainImage } from "../utils/images";
import "./ProductList.css";
import apiUrl from "../api";

// IZMENJENO: za sada vidljivo svima; pre objave vratiti proveru (?admin=1)
const isAdmin = true;

function ProductList({
    search,
    selectedCategory,
    categories = [],
    sortBy = "recommended",
    shouldScrollToProducts = false
}) {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [selectedProductId, setSelectedProductId] = useState(null);

    const productListRef = useRef(null);

    const loadProducts = useCallback(async (silent = false) => {
        try {
            if (!silent) {
                setLoading(true);
                setError("");
            }

            const response = await fetch(`${apiUrl}/api/Product`);

            if (!response.ok) {
                throw new Error("Neuspešno učitavanje proizvoda.");
            }

            const data = await response.json();
            setProducts(data);
        } catch (error) {
            if (!silent) {
                setError(
                    "Ne mogu da učitam proizvode. Proveri da li backend radi i da li je adresa API-ja ispravna."
                );
            }
        } finally {
            if (!silent) {
                setLoading(false);
            }
        }
    }, []);

    useEffect(() => {
        loadProducts();
    }, [loadProducts]);

    // otvoreni proizvod se uvek uzima iz sveže liste
    const selectedProduct =
        products.find((p) => p.id === selectedProductId) ?? null;

    const normalizedSearch = (search ?? "")
        .trim()
        .toLocaleLowerCase("sr");

    const selectedCategoryId = categories.find(
        (category) => category.name === selectedCategory
    )?.id;

    const filteredProducts = products.filter((product) => {
        const productName = (product.name ?? "").toLocaleLowerCase("sr");
        const productDescription = (product.description ?? "")
            .toLocaleLowerCase("sr");

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

    useEffect(() => {
        if (
            shouldScrollToProducts &&
            !loading &&
            !error &&
            sortedProducts.length > 0
        ) {
            productListRef.current?.scrollIntoView({
                behavior: "smooth",
                block: "start"
            });
        }
    }, [
        shouldScrollToProducts,
        loading,
        error,
        sortedProducts.length
    ]);

    return (
        <div className="product-list" ref={productListRef}>
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
                        {sortedProducts.map((product) => {
                            const mainImage = getMainImage(product);

                            return (
                                <article
                                    className="product-card"
                                    key={product.id}
                                    onClick={() => setSelectedProductId(product.id)}
                                    style={{ cursor: "pointer" }}
                                >
                                    {mainImage ? (
                                        <img
                                            className="product-card__image"
                                            src={mainImage.url}
                                            alt={product.name}
                                            loading="lazy"
                                        />
                                    ) : (
                                        <div className="product-card__image product-card__image--empty">
                                            Nema slike
                                        </div>
                                    )}

                                    <h3>{product.name}</h3>

                                    <p>{product.description}</p>

                                    {product.price == null
                                        ? "Cena nije navedena"
                                        : Number(product.price).toLocaleString("sr-RS")} RSD

                                    <p>
                                        Dostupna količina: {product.quantity}
                                    </p>

                                    <p>Status: {product.status}</p>
                                </article>
                            );
                        })}
                    </div>
                )}
            </div>

            {selectedProduct && (
                <ProductProfile
                    product={selectedProduct}
                    isAdmin={isAdmin}
                    onChanged={() => loadProducts(true)}
                    onClose={() => setSelectedProductId(null)}
                />
            )}
        </div>
    );
}

export default ProductList;
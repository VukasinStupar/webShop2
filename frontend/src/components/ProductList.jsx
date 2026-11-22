
import { useCallback, useEffect, useRef, useState } from "react";
import EmptyProducts from "./EmptyProducts";
import ProductProfile from "./ProductProfile";
import { getMainImage } from "../utils/images";
import "./ProductList.css";
import apiUrl from "../api";
import EditProduct from "./EditProduct";
import { CustomButton } from "../customComponents/customComponents";

import { useAuth } from "../context/AuthContext";


function ProductList({
    search,
    selectedCategory,
    categories = [],
    sortBy = "recommended",
    shouldScrollToProducts = false,
    onDeleteProduct,
    onProductChanged
}) {
    const { isAdmin } = useAuth();
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [selectedProductId, setSelectedProductId] = useState(null);
    const [editingProductId, setEditingProductId] = useState(null);

    // Paginacija
    const [page, setPage] = useState(1);
    const [pageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(1);
    const [totalCount, setTotalCount] = useState(0);

    const editingProduct =
        products.find((p) => p.id === editingProductId) ?? null;

    const productListRef = useRef(null);

    const loadProducts = useCallback(async (silent = false, requestedPage = page) => {
        try {
            if (!silent) {
                setLoading(true);
                setError("");
            }

            const response = await fetch(
                `${apiUrl}/api/Product?page=${requestedPage}&pageSize=${pageSize}`
            );

            if (!response.ok) {
                throw new Error("Neuspešno učitavanje proizvoda.");
            }

            const data = await response.json();

            setProducts(data.items ?? []);
            setTotalPages(data.totalPages ?? 1);
            setTotalCount(data.totalCount ?? 0);
            setPage(data.page ?? requestedPage);

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
    }, [page, pageSize]);

    useEffect(() => {
        loadProducts(false, page);
    }, [page, loadProducts]);

    // Promena stranice
    function handlePageChange(newPage) {
        if (newPage < 1 || newPage > totalPages) {
            return;
        }

        setPage(newPage);
        setSelectedProductId(null);
        setEditingProductId(null);
    }

    async function handleDeleteClick(event, productId) {
        event.stopPropagation();

        if (onDeleteProduct) {
            await onDeleteProduct(productId);
        }

        loadProducts(true);
    }

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

            <div className="product-list__content">
                {loading ? (
                    <p>Učitavanje proizvoda...</p>
                ) : error ? (
                    <p>{error}</p>
                ) : sortedProducts.length === 0 ? (
                    <EmptyProducts />
                ) : (
                    <>
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
                                            : new Intl.NumberFormat("sr-RS", {
                                                style: "currency",
                                                currency: "EUR",
                                            }).format(Number(product.price))}

                                        <p>
                                            Dostupna količina: {product.quantity}
                                        </p>

                                        <p>Status: {product.status}</p>

                                        {isAdmin && (
                                            <div className="product-card__actions">

                                                <CustomButton
                                                    className="button-edit-product"
                                                    onClick={(event) => {
                                                        event.stopPropagation();
                                                        setEditingProductId(product.id);
                                                    }}
                                                >
                                                    Izmeni proizvod
                                                </CustomButton>

                                                {onDeleteProduct && (
                                                    <CustomButton
                                                        className="button-delete-product"
                                                        onClick={(event) =>
                                                            handleDeleteClick(event, product.id)
                                                        }
                                                    >
                                                        Obriši proizvod
                                                    </CustomButton>
                                                )}

                                            </div>
                                        )}

                                    </article>
                                );
                            })}
                        </div>

                        {/* PAGINACIJA */}
                        {totalPages > 1 && (
                            <div className="pagination">

                                <CustomButton
                                    disabled={page === 1}
                                    onClick={() => handlePageChange(page - 1)}
                                >
                                    Prethodna
                                </CustomButton>

                                <span>
                                    Stranica {page} od {totalPages}
                                </span>

                                <CustomButton
                                    disabled={page === totalPages}
                                    onClick={() => handlePageChange(page + 1)}
                                >
                                    Sledeća
                                </CustomButton>

                            </div>
                        )}

                        <p className="pagination-info">
                            Ukupno proizvoda: {totalCount}
                        </p>
                    </>
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

            {editingProduct && (
                <EditProduct
                    product={editingProduct}
                    categories={categories}
                    onClose={() => setEditingProductId(null)}
                    onSaved={() => {
                        setEditingProductId(null);
                        loadProducts(true);
                        onProductChanged?.();
                    }}
                />
            )}

        </div>
    );
}

export default ProductList;

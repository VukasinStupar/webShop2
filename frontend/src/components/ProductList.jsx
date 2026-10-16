
import { useEffect, useRef, useState } from "react";
import EmptyProducts from "./EmptyProducts";
import "./ProductList.css";

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

    const productListRef = useRef(null);

    useEffect(() => {
        async function loadProducts() {
            try {
                setLoading(true);
                setError("");

                // const apiUrl = import.meta.env.VITE_API_URL;
                // const response = await fetch(`${apiUrl}/api/Product`);

                const apiUrl = import.meta.env.VITE_API_URL || "https://localhost:51540";
const response = await fetch(`${apiUrl}/api/Product`);

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
                        {sortedProducts.map((product) => (
                            <article
                                className="product-card"
                                key={product.id}
                            >
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
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}

export default ProductList;


//////

// import { useEffect, useRef, useState } from "react";
// import EmptyProducts from "./EmptyProducts";
// import "./ProductList.css";

// function ProductList({
//     search = "",
//     selectedCategory,
//     categories = [],
//     sortBy = "recommended",
//     shouldScrollToProducts = false
// }) {
//     const [products, setProducts] = useState([]);
//     const [loading, setLoading] = useState(true);
//     const [error, setError] = useState("");

//     const productListRef = useRef(null);

//     useEffect(() => {
//         async function loadProducts() {
//             try {
//                 setLoading(true);
//                 setError("");

//                 const apiUrl = import.meta.env.VITE_API_URL;
//                 const response = await fetch(`${apiUrl}/api/Product`);

//                 if (!response.ok) {
//                     throw new Error("Neuspešno učitavanje proizvoda.");
//                 }

//                 const data = await response.json();

//                 if (!Array.isArray(data)) {
//                     throw new Error("API nije vratio listu proizvoda.");
//                 }

//                 setProducts(data);
//             } catch (error) {
//                 console.error("Greška pri učitavanju proizvoda:", error);

//                 setError(
//                     "Ne mogu da učitam proizvode. Proveri da li backend radi i da li je adresa API-ja ispravna."
//                 );
//             } finally {
//                 setLoading(false);
//             }
//         }

//         loadProducts();
//     }, []);

//     const normalizedSearch = search.trim().toLocaleLowerCase("sr");

//     const selectedCategoryId = categories.find(
//         (category) => category.name === selectedCategory
//     )?.id;

//     const filteredProducts = products.filter((product) => {
//         const productName = (product.name ?? "").toLocaleLowerCase("sr");
//         const productDescription = (
//             product.description ?? ""
//         ).toLocaleLowerCase("sr");

//         const matchesSearch =
//             productName.includes(normalizedSearch) ||
//             productDescription.includes(normalizedSearch);

//         const matchesCategory =
//             !selectedCategory ||
//             selectedCategory === "Sve kategorije" ||
//             Number(product.categoryId) === Number(selectedCategoryId);

//         return matchesSearch && matchesCategory;
//     });

//     const sortedProducts = [...filteredProducts].sort((a, b) => {
//         switch (sortBy) {
//             case "price-asc":
//                 return Number(a.price) - Number(b.price);

//             case "price-desc":
//                 return Number(b.price) - Number(a.price);

//             case "newest":
//                 return Number(b.id) - Number(a.id);

//             case "recommended":
//             default:
//                 return 0;
//         }
//     });

//     useEffect(() => {
//         if (
//             shouldScrollToProducts &&
//             !loading &&
//             !error &&
//             sortedProducts.length > 0
//         ) {
//             productListRef.current?.scrollIntoView({
//                 behavior: "smooth",
//                 block: "start"
//             });
//         }
//     }, [
//         shouldScrollToProducts,
//         loading,
//         error,
//         sortedProducts.length
//     ]);

//     // Najveća količina služi kao osnova za relativne progres barove.
//     const maxQuantity = Math.max(
//         0,
//         ...filteredProducts.map((product) => {
//             const quantity = Number(product.quantity);

//             return Number.isFinite(quantity) ? Math.max(0, quantity) : 0;
//         })
//     );

//     function formatPrice(price) {
//         if (
//             price === null ||
//             price === undefined ||
//             price === "" ||
//             !Number.isFinite(Number(price))
//         ) {
//             return null;
//         }

//         return `${Number(price).toLocaleString("sr-RS", {
//             maximumFractionDigits: 2
//         })} RSD`;
//     }

//     function getQuantity(quantity) {
//         const parsedQuantity = Number(quantity);

//         if (
//             quantity === null ||
//             quantity === undefined ||
//             quantity === "" ||
//             !Number.isFinite(parsedQuantity)
//         ) {
//             return null;
//         }

//         return Math.max(0, parsedQuantity);
//     }

//     function getStatusClass(status) {
//         const normalizedStatus = String(status ?? "")
//             .trim()
//             .toLocaleLowerCase("sr");

//         if (
//             normalizedStatus.includes("dostup") ||
//             normalizedStatus.includes("aktivan")
//         ) {
//             return "product-card__status--available";
//         }

//         if (
//             normalizedStatus.includes("nema") ||
//             normalizedStatus.includes("nedostup") ||
//             normalizedStatus.includes("rasprodat")
//         ) {
//             return "product-card__status--unavailable";
//         }

//         return "product-card__status--default";
//     }

//     return (
//         <div className="product-list" ref={productListRef}>
//             <div className="product-list__toolbar">
//                 <div className="product-list__result">
//                     <span className="product-list__result-label">
//                         {loading ? "Učitavanje ponude" : "Trenutna ponuda"}
//                     </span>

//                     <span className="product-list__result-value">
//                         {loading
//                             ? "..."
//                             : `${filteredProducts.length} ${
//                                   filteredProducts.length === 1
//                                       ? "proizvod"
//                                       : "proizvoda"
//                               }`}
//                     </span>
//                 </div>

//                 {normalizedSearch && (
//                     <div className="product-list__search">
//                         Pretraga: <strong>{search.trim()}</strong>
//                     </div>
//                 )}
//             </div>

//             <div className="product-list__content">
//                 {loading ? (
//                     <p>Učitavanje proizvoda...</p>
//                 ) : error ? (
//                     <p>{error}</p>
//                 ) : sortedProducts.length === 0 ? (
//                     <EmptyProducts />
//                 ) : (
//                     <div className="product-grid">
//                         {sortedProducts.map((product) => {
//                             const price = formatPrice(product.price);
//                             const quantity = getQuantity(product.quantity);

//                             const quantityPercent =
//                                 quantity !== null && maxQuantity > 0
//                                     ? Math.min(
//                                           100,
//                                           (quantity / maxQuantity) * 100
//                                       )
//                                     : 0;

//                             const status = String(
//                                 product.status ?? "Status nije naveden"
//                             );

//                             return (
//                                 <article
//                                     className="product-card"
//                                     key={product.id}
//                                 >
//                                     <div className="product-card__top">
//                                         <span className="product-card__tag">
//                                             PROIZVOD
//                                         </span>

//                                         <span
//                                             className={`product-card__status ${getStatusClass(
//                                                 product.status
//                                             )}`}
//                                         >
//                                             {status}
//                                         </span>
//                                     </div>

//                                     <h3>{product.name}</h3>

//                                     <p className="product-card__description">
//                                         {product.description ||
//                                             "Opis proizvoda nije dostupan."}
//                                     </p>

//                                     <div className="product-card__price">
//                                         {price ?? "Cena nije navedena"}
//                                     </div>

//                                     <div className="product-card__stock">
//                                         <div className="product-card__stock-heading">
//                                             <span>Dostupna količina</span>

//                                             <strong>
//                                                 {quantity === null
//                                                     ? "Nije navedeno"
//                                                     : quantity.toLocaleString(
//                                                           "sr-RS"
//                                                       )}
//                                             </strong>
//                                         </div>

//                                         <div
//                                             className="product-card__progress"
//                                             role="progressbar"
//                                             aria-label={`Relativna količina proizvoda ${
//                                                 product.name ?? ""
//                                             }`}
//                                             aria-valuemin={0}
//                                             aria-valuemax={100}
//                                             aria-valuenow={quantityPercent}
//                                         >
//                                             <span
//                                                 className="product-card__progress-bar"
//                                                 style={{
//                                                     width: `${quantityPercent}%`
//                                                 }}
//                                             />
//                                         </div>

//                                         {quantity !== null &&
//                                             maxQuantity > 0 && (
//                                                 <span className="product-card__stock-note">
//                                                     {quantity === maxQuantity
//                                                         ? "Najveća količina u trenutnoj ponudi"
//                                                         : "U poređenju sa najvećom količinom"}
//                                                 </span>
//                                             )}
//                                     </div>
//                                 </article>
//                             );
//                         })}
//                     </div>
//                 )}
//             </div>
//         </div>
//     );
// }

// export default ProductList;

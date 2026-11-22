import { useEffect, useRef, useState } from "react";
import ProductList from "./ProductList";
import "./Marketplace.css";
import { CustomButton } from "../customComponents/customComponents";

function Marketplace({
    search,
    categories,
    selectedCategory,
    onCategoryChange,
    sortBy,
    onDeleteProduct,
    onProductChanged
}) {
    const [shouldScrollToProducts, setShouldScrollToProducts] = useState(false);

    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    const previousFiltersRef = useRef({
        search,
        selectedCategory
    });

    useEffect(() => {
        const previousFilters = previousFiltersRef.current;

        const filtersChanged =
            previousFilters.search !== search ||
            previousFilters.selectedCategory !== selectedCategory;

        if (filtersChanged) {
            setShouldScrollToProducts(true);
            setCurrentPage(1);
        }

        previousFiltersRef.current = {
            search,
            selectedCategory
        };
    }, [search, selectedCategory]);

    return (
        <section className="marketplace" id="marketplace">
            <div className="marketplace__heading">
                <div>
                    <span className="marketplace__eyebrow">
                        TRŽIŠTE PROIZVODA
                    </span>

                    <h2 className="marketplace__title">
                        Istražite ponudu
                    </h2>

                    <p className="marketplace__description">
                        Pronađite proizvode prema svojim potrebama.
                    </p>
                </div>
            </div>

            <div className="marketplace__layout">
                <ProductList
                    search={search}
                    categories={categories}
                    selectedCategory={selectedCategory}
                    sortBy={sortBy}
                    shouldScrollToProducts={shouldScrollToProducts}
                    onDeleteProduct={onDeleteProduct}
                    onProductChanged={onProductChanged}
                    currentPage={currentPage}
                    onTotalPagesChange={setTotalPages}
                />
            </div>

            {totalPages > 1 && (
                <div>
                    
                    <CustomButton
                        onClick={() => setCurrentPage(currentPage - 1)}
                        disabled={currentPage === 1}
                    >
                        Prethodna
                    </CustomButton>

                    <span className="marketplace__pagination-info">
                        Stranica {currentPage} od {totalPages}
                    </span>

                    <CustomButton
                        
                        onClick={() => setCurrentPage(currentPage + 1)}
                        disabled={currentPage === totalPages}
                    >
                        Sledeća
                    </CustomButton>
                </div>
            )}
        </section>
    );
}

export default Marketplace;
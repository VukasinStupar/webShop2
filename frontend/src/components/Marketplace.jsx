
import { useState } from "react";
import FilterSidebar from "./FilterSidebar";
import ProductList from "./ProductList";
import "./Marketplace.css";

function Marketplace({
    search,
    categories,
    selectedCategory,
    onCategoryChange,
}) {
    const [sortBy, setSortBy] = useState("recommended");

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
                <FilterSidebar
                    categories={categories}
                    selectedCategory={selectedCategory}
                    onCategoryChange={onCategoryChange}
                    sortBy={sortBy}
                    onSortChange={setSortBy}
                />

                <ProductList
                    search={search}
                    categories={categories}
                    selectedCategory={selectedCategory}
                    sortBy={sortBy}
                />

                
            </div>
        </section>
    );
}

export default Marketplace;


import "./CategoryList.css";

function CategoryList({
    categories,
    selectedCategory,
    onCategoryChange
}) {
    const allCategories = [
        { id: 0, name: "Sve kategorije", symbol: "◈" },
        ...categories.map((category) => ({
            ...category,
            symbol: "▣"
        }))
    ];

    return (
        <section className="category-list" aria-labelledby="category-list-title">
            <div className="category-list__heading">
                <div>
                    <span className="category-list__eyebrow">
                        
                    </span>

                    <h2
                        className="category-list__title"
                        id="category-list-title"
                    >
                        
                    </h2>
                </div>

                <span className="category-list__count">
                    {categories.length} kategorija
                </span>
            </div>

            <div className="category-list__items">
                {allCategories.map((category) => (
                    <button
                        className={[
                            "category-list__item",
                            selectedCategory === category.name
                                ? "category-list__item--active"
                                : "",
                        ].filter(Boolean).join(" ")}
                        key={category.id}
                        type="button"
                        onClick={() => onCategoryChange(category.name)}
                        aria-pressed={selectedCategory === category.name}
                    >
                        <span
                            className="category-list__icon"
                            aria-hidden="true"
                        >
                            {category.symbol}
                        </span>

                        <span className="category-list__name">
                            {category.name}
                        </span>

                        <span
                            className="category-list__arrow"
                            aria-hidden="true"
                        >
                            →
                        </span>
                    </button>
                ))}
            </div>
        </section>
    );
}

export default CategoryList;

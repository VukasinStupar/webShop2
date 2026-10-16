
import "./FilterSidebar.css";

function FilterSidebar({
    categories,
    selectedCategory,
    onCategoryChange,
    sortBy,
    onSortChange
}) {
    return (
        <aside className="filter-sidebar" aria-label="Filteri proizvoda">
            <div className="filter-sidebar__heading">
                <h3 className="filter-sidebar__title">Filteri</h3>
                <span className="filter-sidebar__icon" aria-hidden="true">
                    ☷
                </span>
            </div>

            <div className="filter-sidebar__section">
                <label
                    className="filter-sidebar__label"
                    htmlFor="filter-category"
                >
                    Kategorija
                </label>

                <select
                    className="filter-sidebar__select"
                    id="filter-category"
                    value={selectedCategory}
                    onChange={(event) =>
                        onCategoryChange(event.target.value)
                    }
                >
                    <option value="Sve kategorije">Sve kategorije</option>

                    {(categories ?? []).map((category) => (
                        <option key={category.id} value={category.name}>
                            {category.name}
                        </option>
                    ))}
                </select>
            </div>

            <div className="filter-sidebar__section">
                <label
                    className="filter-sidebar__label"
                    htmlFor="filter-sort"
                >
                    Sortiranje
                </label>

                <select
                    className="filter-sidebar__select"
                    id="filter-sort"
                    value={sortBy}
                    onChange={(event) => onSortChange(event.target.value)}
                >
                    <option value="recommended">Preporučeno</option>
                    <option value="price-asc">
                        Cena: od niže ka višoj
                    </option>
                    <option value="price-desc">
                        Cena: od više ka nižoj
                    </option>
                    <option value="newest">Najnovije</option>
                </select>
            </div>

            <div className="filter-sidebar__info">
                <span className="filter-sidebar__info-icon" aria-hidden="true">
                    i
                </span>
                <p>Izaberite kategoriju da biste suzili ponudu proizvoda.</p>
            </div>
        </aside>
    );
}

export default FilterSidebar;

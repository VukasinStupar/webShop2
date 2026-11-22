
import "./FilterSidebar.css";
import { CustomSelect } from "../customComponents/customComponents";

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

                <CustomSelect
                    className="filter-sidebar__select"
                    value={selectedCategory}
                    onChange={onCategoryChange}
                    options={[
                        { value: "Sve kategorije", label: "Sve kategorije" },
                        ...(categories ?? []).map((category) => ({
                            value: category.name,
                            label: category.name
                        }))
                    ]}
                />
            </div>

            <div className="filter-sidebar__section">
                <label
                    className="filter-sidebar__label"
                    htmlFor="filter-sort"
                >
                    Sortiranje
                </label>

                <CustomSelect
                    className="filter-sidebar__select"
                    value={sortBy}
                    onChange={onSortChange}
                    options={[
                        { value: "recommended", label: "Preporučeno" },
                        {
                            value: "price-asc",
                            label: "Cena: od niže ka višoj"
                        },
                        {
                            value: "price-desc",
                            label: "Cena: od više ka nižoj"
                        },
                        { value: "newest", label: "Najnovije" }
                    ]}
                />
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

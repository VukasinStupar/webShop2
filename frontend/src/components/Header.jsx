
import "./Header.css";
import CategoryList from "./CategoryList";

function Header({
    search,
    onSearchChange,
    onAddProduct,
    categories,
    selectedCategory,
    onCategoryChange,
    sortBy,
    onSortChange
}) {
    function handleSubmit(event) {
        event.preventDefault();
    }

    return (
        <header className="site-header">
            <div className="site-header__inner">
                <a className="site-header__brand" href="/">
                    <span className="site-header__brand-icon">W</span>
                    <span className="site-header__brand-name">
                        WEB SHOP
                    </span>
                </a>

                <nav
                    className="site-header__nav"
                    aria-label="Glavna navigacija"
                >
                    <a
                        className="site-header__nav-link site-header__nav-link--active"
                        href="/"
                    >
                        Početna
                    </a>

                    <a
                        className="site-header__nav-link"
                        href="#marketplace"
                    >
                        Oglasi
                    </a>

                    <a
                        className="site-header__nav-link"
                        href="#footer"
                    >
                        Kontakt
                    </a>
                </nav>

                <form
                    className="site-header__search"
                    onSubmit={handleSubmit}
                >
                    <span
                        className="site-header__search-icon"
                        aria-hidden="true"
                    >
                        ⌕
                    </span>

                    <input
                        className="site-header__search-input"
                        type="search"
                        value={search}
                        onChange={(event) =>
                            onSearchChange(event.target.value)
                        }
                        placeholder="Šta tražite?"
                        aria-label="Pretraži proizvode"
                    />

                    <button
                        className="site-header__search-button"
                        type="submit"
                    >
                        Pretraži
                    </button>
                </form>

                <button
                    className="site-header__add-product"
                    type="button"
                    onClick={onAddProduct}
                >
                    <span aria-hidden="true">+</span>
                    Dodaj proizvod
                </button>
            </div>

            <div className="site-header__categories">
                <CategoryList
                    categories={categories}
                    selectedCategory={selectedCategory}
                    onCategoryChange={onCategoryChange}
                />

                <div className="site-header__sort">
                    <label
                        className="site-header__sort-label"
                        htmlFor="header-sort"
                    >
                        Sortiraj:
                    </label>

                    <select
                        className="site-header__sort-select"
                        id="header-sort"
                        value={sortBy}
                        onChange={(event) =>
                            onSortChange(event.target.value)
                            
                        }
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
            </div>
        </header>
    );
}

export default Header;

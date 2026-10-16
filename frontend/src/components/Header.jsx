import "./Header.css";

function Header({ search, onSearchChange }) {
    function handleSubmit(event) {
        event.preventDefault();
    }

    return (
        <header className="site-header">
            <div className="site-header__inner">
                <a className="site-header__brand" href="/">
                    <span className="site-header__brand-icon">W</span>
                    <span className="site-header__brand-name">WEB SHOP</span>
                </a>

                <nav className="site-header__nav" aria-label="Glavna navigacija">
                    <a className="site-header__nav-link site-header__nav-link--active" href="/">
                        Početna
                    </a>
                    <a className="site-header__nav-link" href="#marketplace">
                        Oglasi
                    </a>
                    <a className="site-header__nav-link" href="#footer">
                        Kontakt
                    </a>
                </nav>

                <form className="site-header__search" onSubmit={handleSubmit}>
                    <span className="site-header__search-icon" aria-hidden="true">
                        ⌕
                    </span>

                    <input
                        className="site-header__search-input"
                        type="search"
                        value={search}
                        onChange={(event) => onSearchChange(event.target.value)}
                        placeholder="Šta tražite?"
                        aria-label="Pretraži proizvode"
                    />

                    <button className="site-header__search-button" type="submit">
                        Pretraži
                    </button>
                </form>
            </div>
        </header>
    );
}

export default Header;
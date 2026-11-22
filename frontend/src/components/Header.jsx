
import { useEffect, useState } from "react";
import "./Header.css";
import CategoryList from "./CategoryList";

import {
    CustomButton,
    CustomSearch,
    CustomSelect
} from "../customComponents/customComponents";

import { useAuth } from "../context/AuthContext";


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
    const [searchValue, setSearchValue] = useState(search ?? "");
    const { isAdmin, logout } = useAuth();
    // Sinhronizacija ako se pretraga promeni spolja
    useEffect(() => {
        setSearchValue(search ?? "");
    }, [search]);

    // Debounce pretrage - 400ms
    useEffect(() => {
        if (searchValue === (search ?? "")) {
            return;
        }

        const timeoutId = setTimeout(() => {
            onSearchChange(searchValue);
        }, 400);

        return () => {
            clearTimeout(timeoutId);
        };
    }, [searchValue, search, onSearchChange]);

    function handleSubmit(event) {
        event.preventDefault();
    }

    const categoryOptions = categories.map((cat) => ({
        value: cat.name,
        label: cat.name
    }));

    const sortOptions = [
        {
            value: "recommended",
            label: "Preporučeno"
        },
        {
            value: "price-asc",
            label: "Cena: od niže ka višoj"
        },
        {
            value: "price-desc",
            label: "Cena: od više ka nižoj"
        },
        {
            value: "newest",
            label: "Najnovije"
        }
    ];

    return (
        <header className="site-header">
            <div className="site-header__inner">
                <div className="site-header__brand">
                    <a className="site-header__brand-icon" href="/">
                        D
                    </a>

                    <div className="site-header__brand-info">
                        <a className="site-header__brand-name" href="/">
                            DUKA SHOP
                        </a>

                        <a
                            className="site-header__brand-phone"
                            href="tel:+381641360754"
                        >
                            +381 64 136 0754
                        </a>
                    </div>
                </div>

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
                    <CustomSearch
                        className="site-header__search-input"
                        value={searchValue}
                        onChange={setSearchValue}
                        placeholder="Šta tražite?"
                    />

                    <CustomButton
                        className="site-header__search-button"
                        type="submit"
                    >
                        Pretraži
                    </CustomButton>
                </form>

                {/* <CustomButton
                    className="site-header__add-product"
                    onClick={onAddProduct}
                >
                    <span aria-hidden="true">+</span>
                    Dodaj proizvod
                </CustomButton> */}

                {isAdmin && (
                    <CustomButton
                        className="site-header__add-product"
                        onClick={onAddProduct}
                    >
                        <span aria-hidden="true">+</span>
                        Dodaj proizvod
                    </CustomButton>
                )}

                {isAdmin && (
                    <CustomButton
                        className="site-header__logout"
                        onClick={logout}
                    >
                        <span aria-hidden="true">↪</span>
                        Odjavi se
                    </CustomButton>
                )}
            </div>

            <div className="site-header__categories">
                <CategoryList
                    categories={categories}
                    selectedCategory={selectedCategory}
                    onCategoryChange={onCategoryChange}
                />

                <div className="site-header__sort site-header__sort--category">
                    <label
                        className="site-header__sort-label"
                        htmlFor="header-category"
                    >
                        Kategorija:
                    </label>

                    <CustomSelect
                        className="site-header__sort-select"
                        value={selectedCategory}
                        onChange={onCategoryChange}
                        options={[
                            {
                                value: "Sve kategorije",
                                label: "Sve kategorije"
                            },
                            ...categoryOptions
                        ]}
                        placeholder="Izaberi kategoriju"
                    />
                </div>

                <div className="site-header__sort">
                    <label
                        className="site-header__sort-label"
                        htmlFor="header-sort"
                    >
                        Sortiraj:
                    </label>

                    <CustomSelect
                        className="site-header__sort-select"
                        value={sortBy}
                        onChange={onSortChange}
                        options={sortOptions}
                        placeholder="Sortiraj"
                    />
                </div>
            </div>
        </header>
    );
}

export default Header;

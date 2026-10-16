
import "./Footer.css";

function Footer() {
    return (
        <footer className="site-footer" id="footer">
            <div className="site-footer__inner">
                <a className="site-footer__brand" href="/">
                    <span className="site-footer__brand-icon">D</span>
                    <span>DUKA SHOP</span>
                </a>

                <p className="site-footer__description">
                    Duka Shop — sve što vas zanima na jednom mestu!
                    Bela tehnika, telefoni, televizori, klime, frižideri,
                    zamrzivači, automobili i još mnogo toga.
                </p>

                <div className="site-footer__contact">
                    <h3>Kontakt</h3>

                    <p>
                        <strong>Ime i prezime:</strong> Dušan Kukić
                    </p>

                    <p>
                        <strong>Telefon:</strong>{" "}
                        <a href="tel:+381641360754">
                            064 136 0754
                        </a>
                    </p>

                    <p>
                        <strong>Firma:</strong> Duka Shop
                    </p>
                </div>

                <div className="site-footer__bottom">
                    <span>
                        © {new Date().getFullYear()} Duka Shop. Sva prava
                        zadržana.
                    </span>

                    <span className="site-footer__note">
                        Sve što vam treba, na jednom mestu.
                    </span>
                </div>
            </div>
        </footer>
    );
}

export default Footer;

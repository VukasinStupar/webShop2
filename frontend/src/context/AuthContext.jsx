
import {
    createContext,
    useCallback,
    useContext,
    useEffect,
    useMemo,
    useState
} from "react";

import authService from "../services/authService";

const AuthContext = createContext(null);

const STORAGE_KEY = "webshop_auth";

function getStoredAuth() {
    try {
        const storedAuth = sessionStorage.getItem(STORAGE_KEY);

        if (!storedAuth) {
            return null;
        }

        const authData = JSON.parse(storedAuth);

        if (
            !authData.token ||
            authData.role !== "Admin" ||
            !authData.expiresAt ||
            new Date(authData.expiresAt).getTime() <= Date.now()
        ) {
            sessionStorage.removeItem(STORAGE_KEY);
            return null;
        }

        return authData;
    } catch {
        sessionStorage.removeItem(STORAGE_KEY);
        return null;
    }
}

export function AuthProvider({ children }) {
    const [authData, setAuthData] = useState(getStoredAuth);

    const logout = useCallback(() => {
        sessionStorage.removeItem(STORAGE_KEY);
        setAuthData(null);
    }, []);

    const saveAuth = useCallback((response) => {
        if (
            !response?.token ||
            !response?.expiresAt ||
            response?.role !== "Admin"
        ) {
            throw new Error(
                "Neispravan odgovor ili korisnik nema Admin ulogu."
            );
        }

        const newAuthData = {
            token: response.token,
            expiresAt: response.expiresAt,
            userId: response.userId,
            email: response.email,
            firstName: response.firstName,
            lastName: response.lastName,
            role: response.role
        };

        sessionStorage.setItem(
            STORAGE_KEY,
            JSON.stringify(newAuthData)
        );

        setAuthData(newAuthData);
    }, []);

    const login = useCallback(async (email, password) => {
        const response = await authService.login(email, password);

        saveAuth(response);

        return response;
    }, [saveAuth]);

    const register = useCallback(async (registerData) => {
        const response = await authService.register(registerData);

        saveAuth(response);

        return response;
    }, [saveAuth]);

    const isAuthenticated =
        Boolean(authData?.token) &&
        Boolean(authData?.expiresAt) &&
        new Date(authData.expiresAt).getTime() > Date.now();

    const isAdmin =
        isAuthenticated &&
        authData?.role === "Admin";

    useEffect(() => {
        if (!authData?.expiresAt) {
            return;
        }

        const remainingTime =
            new Date(authData.expiresAt).getTime() - Date.now();

        if (remainingTime <= 0) {
            logout();
            return;
        }

        const timeoutId = setTimeout(() => {
            logout();
        }, remainingTime);

        return () => clearTimeout(timeoutId);
    }, [authData, logout]);

    const value = useMemo(() => ({
        authData: isAuthenticated ? authData : null,
        token: isAuthenticated ? authData.token : null,
        isAuthenticated,
        isAdmin,
        login,
        register,
        logout
    }), [
        authData,
        isAuthenticated,
        isAdmin,
        login,
        register,
        logout
    ]);

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const context = useContext(AuthContext);

    if (!context) {
        throw new Error(
            "useAuth mora biti korišćen unutar AuthProvider."
        );
    }

    return context;
}

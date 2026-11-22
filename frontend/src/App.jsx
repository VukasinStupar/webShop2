
import { Routes, Route, Navigate } from "react-router-dom";

import HomePage from "./HomePage";
import AdminLogin from "./pages/AdminLogin";
import AdminRegister from "./pages/AdminRegister";
import AdminDashboard from "./pages/AdminDashboard";
import AdminRoute from "./components/AdminRoute";
import { useAuth } from "./context/AuthContext";

function App() {
    const { isAuthenticated, isAdmin } = useAuth();

    return (
        <Routes>
            {/* Javni deo prodavnice */}
            <Route path="/" element={<HomePage />} />

            {/* Administratorska prijava */}
            <Route
                path="/admin/login"
                element={
                    isAuthenticated && isAdmin
                        ? <Navigate to="/admin" replace />
                        : <AdminLogin />
                }
            />

            {/* Registracija administratora */}
            <Route
    path="/admin/register"
    element={<AdminRegister />}
/>

            {/* Zaštićene administratorske stranice */}
            <Route element={<AdminRoute />}>
                <Route
                    path="/admin"
                    element={<AdminDashboard />}
                />
            </Route>

            {/* Nepostojeće rute */}
            <Route path="*" element={<Navigate to="/" replace />} />

            
        </Routes>
    );
}

export default App;

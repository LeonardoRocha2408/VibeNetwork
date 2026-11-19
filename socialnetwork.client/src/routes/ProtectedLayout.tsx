import { Outlet } from "react-router-dom";
import Header from "../components/header";
import Navbar from "../components/navbar";

export default function ProtectedLayout() {
    return (
        <>
            <div className="md:ml-25">
            <Header/>
            </div>
            <Navbar />
            <main className="md:ml-25">
                <Outlet />
            </main>
        </>
    )
}
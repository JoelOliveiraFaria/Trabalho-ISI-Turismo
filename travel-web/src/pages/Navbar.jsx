import { Link, useNavigate } from "react-router-dom";

function Navbar() {
  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  const handleLogout = () => {
    localStorage.clear();
    navigate("/login");
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-primary shadow-sm px-4">
      <div className="container">
        <Link className="navbar-brand fw-bold" to="/">
          ✈️ Travel Planner
        </Link>
        
        <div className="d-flex gap-2">
          {token ? (
            <>
              <Link to="/trips" className="btn btn-light text-primary fw-bold me-2">
                Minhas Viagens
              </Link>
              <button onClick={handleLogout} className="btn btn-outline-light">
                Sair
              </button>
            </>
          ) : (
            <Link to="/login" className="btn btn-light text-primary">Entrar</Link>
          )}
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
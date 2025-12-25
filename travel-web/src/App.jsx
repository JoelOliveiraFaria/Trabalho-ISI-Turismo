import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";
import CreateTrip from "./pages/CreateTrip";
import Trips from "./pages/Trips";
import Home from "./pages/Home";
import EditTrip from "./pages/EditTrip";

function PrivateRoute({ children }) {
  const token = localStorage.getItem("token");
  return token ? children : <Navigate to="/" />;
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />

        <Route path="/login" element={<Login />} />

        <Route path="/register" element={<Register />} />

        {/* Rota Protegida: Viagens (Vamos criar a seguir) */}
        <Route 
          path="/trips" 
          element={ 
          <PrivateRoute>
            <Trips /> 
          </PrivateRoute>
        } 
        />

        <Route path="/create-trip" element={   
          <PrivateRoute> 
            <CreateTrip />
          </PrivateRoute>
          }
        />
          <Route path="/edit-trip/:id" element={   
          <PrivateRoute> 
            <EditTrip />
          </PrivateRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
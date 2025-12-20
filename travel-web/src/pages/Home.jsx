import { Link } from "react-router-dom";

function Home() {
  const isLoggedIn = !!localStorage.getItem("token"); // Verifica se já tem login

  return (
    <div className="d-flex flex-column min-vh-100">
      
      {/* 1. NAVBAR (Barra de Topo) */}
      <nav className="navbar navbar-expand-lg navbar-light bg-white shadow-sm px-4">
        <div className="container-fluid">
          <Link className="navbar-brand fw-bold text-primary" to="/">
            ✈️ Travel Planner
          </Link>
          
          <div className="d-flex gap-2">
            {isLoggedIn ? (
              <Link to="/trips" className="btn btn-primary">
                As minhas Viagens
              </Link>
            ) : (
              <>
                <Link to="/login" className="btn btn-outline-primary">Entrar</Link>
                <Link to="/register" className="btn btn-primary">Registar</Link>
              </>
            )}
          </div>
        </div>
      </nav>

      {/* 2. HERO SECTION (Destaque Principal) */}
      <div className="bg-light text-center py-5 mb-5" style={{backgroundImage: 'linear-gradient(to right, #eef2f3, #8e9eab)'}}>
        <div className="container py-5">
          <h1 className="display-4 fw-bold text-dark mb-3">
            Planeia a tua próxima aventura
          </h1>
          <p className="lead text-secondary mb-4">
            Organiza itinerários, controla o orçamento e verifica a meteorologia num só lugar.
            <br />
            Evita conflitos na agenda com a nossa integração inteligente.
          </p>
          
          {isLoggedIn ? (
            <Link to="/trips" className="btn btn-lg btn-success px-5 shadow">
              Ir para o Dashboard 🚀
            </Link>
          ) : (
            <Link to="/register" className="btn btn-lg btn-primary px-5 shadow">
              Começar Agora (Grátis)
            </Link>
          )}
        </div>
      </div>

      {/* 3. FEATURES (Funcionalidades) */}
      <div className="container mb-5">
        <div className="row text-center">
          
          {/* Card 1 */}
          <div className="col-md-4 mb-4">
            <div className="card h-100 border-0 shadow-sm p-3">
              <div className="card-body">
                <div className="display-4 mb-3">📅</div>
                <h5 className="card-title fw-bold">Agenda Inteligente</h5>
                <p className="card-text text-muted">
                  Ligado ao Google Calendar. Avisamos-te se marcares férias no dia de um exame ou reunião importante.
                </p>
              </div>
            </div>
          </div>

          {/* Card 2 */}
          <div className="col-md-4 mb-4">
            <div className="card h-100 border-0 shadow-sm p-3">
              <div className="card-body">
                <div className="display-4 mb-3">⛅</div>
                <h5 className="card-title fw-bold">Meteorologia</h5>
                <p className="card-text text-muted">
                  Previsão automática do tempo para o teu destino. Não te deixes apanhar pela chuva!
                </p>
              </div>
            </div>
          </div>

          {/* Card 3 */}
          <div className="col-md-4 mb-4">
            <div className="card h-100 border-0 shadow-sm p-3">
              <div className="card-body">
                <div className="display-4 mb-3">💰</div>
                <h5 className="card-title fw-bold">Controlo de Custos</h5>
                <p className="card-text text-muted">
                  Define um orçamento, regista despesas e calcula automaticamente o custo do seguro de viagem.
                </p>
              </div>
            </div>
          </div>

        </div>
      </div>

      {/* 4. FOOTER (Rodapé) */}
      <footer className="mt-auto py-3 bg-dark text-white text-center">
        <div className="container">
          <small>&copy; 2025 Travel Planner ISI. Feito por Joel.</small>
        </div>
      </footer>

    </div>
  );
}

export default Home;
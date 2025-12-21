import React from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Header from './components/Layout/Header';
import Footer from './components/Layout/Footer';
import Sidebar from './components/Layout/Sidebar';
import Login from './components/Auth/Login';
import Register from './components/Auth/Register';
import VoterDashboard from './components/Voter/VoterDashboard';
import CandidateDashboard from './components/Candidate/CandidateDashboard';
import ProtectedRoute from './components/Auth/ProtectedRoute';
import './styles/globals.css';

const App = () => {
  return (
    <Router>
      <Header />
      <Sidebar />
      <main>
        <Switch>
          <Route path="/login" component={Login} />
          <Route path="/register" component={Register} />
          <ProtectedRoute path="/voter/dashboard" component={VoterDashboard} />
          <ProtectedRoute path="/candidate/dashboard" component={CandidateDashboard} />
          {/* Add more routes as needed */}
        </Switch>
      </main>
      <Footer />
    </Router>
  );
};

export default App;
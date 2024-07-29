import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import SearchForm from './components/SearchForm';
import Header from './components/Header';

function App() {
    return (
        <Router>
            <div>
                <Header />
                <Routes>
                    <Route path="/" element={<SearchForm />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
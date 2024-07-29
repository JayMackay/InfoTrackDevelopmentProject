import React, { useState } from 'react';
import '../styles.css';
import SearchEngineDropdown from './SearchEngineDropdown';

const SearchForm = () => {
    const [keywords, setKeywords] = useState('');
    const [url, setUrl] = useState('');
    const [result, setResult] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [selectedEngine, setSelectedEngine] = useState('google'); // Default search engine

    const handleSubmit = async (event) => {
        event.preventDefault();

        if (!keywords || !url) {
            setError('Both fields are required.');
            return;
        }

        setError('');
        setLoading(true);
        setResult(''); // Clear previous results

        try {
            // Add a unique query parameter to avoid caching issues
            const timestamp = new Date().getTime();
            const response = await fetch(`/api/search?ts=${timestamp}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ keywords, url, searchEngine: selectedEngine }), // Include selectedEngine
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();

            // Check the result
            if (data.positions.includes(-1)) {
                // Check if -1 is due to rate limit
                if (data.error && data.error.includes('rate limit exceeded')) {
                    setResult('Search engine rate limit exceeded. Please try again later.');
                } else {
                    setResult('No valid URL found with the matching phrase.');
                }
            } else if (data.positions.length === 0) {
                setResult('No results found.');
            } else {
                setResult(data.positions.join(', '));
            }
        } catch (error) {
            setError(`Error: ${error.message}`);
        } finally {
            setLoading(false);

            // Reset form fields after submission
            setKeywords('');
            setUrl('');
        }
    };

    return (
        <div className="search-form">
            <h1>SEO Search</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="keywords">Keywords:</label>
                    <input
                        id="keywords"
                        type="text"
                        value={keywords}
                        onChange={(e) => setKeywords(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="url">URL:</label>
                    <input
                        id="url"
                        type="text"
                        value={url}
                        onChange={(e) => setUrl(e.target.value)}
                        required
                    />
                </div>
                <SearchEngineDropdown
                    selectedEngine={selectedEngine}
                    onEngineChange={setSelectedEngine} // Handle engine change
                />
                <button type="submit" disabled={loading}>
                    {loading ? 'Searching...' : 'Search'}
                </button>
            </form>
            {error && <div className="error">{error}</div>}
            {result && (
                <div>
                    <h2>Result</h2>
                    <p>{result}</p>
                </div>
            )}
        </div>
    );
};

export default SearchForm;
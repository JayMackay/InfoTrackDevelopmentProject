import React, { useState } from 'react';
import '../styles.css';

const SearchForm = () => {
    const [keywords, setKeywords] = useState('');
    const [url, setUrl] = useState('');
    const [result, setResult] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

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
                body: JSON.stringify({ keywords, url }),
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();

            // Check if result contains -1
            if (data.positions.includes(-1)) {
                setResult('No valid URL found with the matching phrase or rate limit exceeded.');
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
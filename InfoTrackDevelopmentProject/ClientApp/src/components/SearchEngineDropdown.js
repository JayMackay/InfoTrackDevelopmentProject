import React from 'react';
import PropTypes from 'prop-types';

const SearchEngineDropdown = ({ selectedEngine, onEngineChange }) => {
    const handleChange = (event) => {
        onEngineChange(event.target.value);
    };

    return (
        <div>
            <label htmlFor="search-engine">Search Engine: </label>
            <select
                id="search-engine"
                value={selectedEngine}
                onChange={handleChange}
            >
                <option value="google">Google</option>
                <option value="bing">Bing</option>
                <option value="yahoo">Yahoo</option>
                {/* Add other options here */}
            </select>
        </div>
    );
};

// Add propTypes validation
SearchEngineDropdown.propTypes = {
    selectedEngine: PropTypes.string.isRequired,
    onEngineChange: PropTypes.func.isRequired,
};

export default SearchEngineDropdown;
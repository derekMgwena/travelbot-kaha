import React, { Component } from 'react';
import React, { useState, useEffect } from 'react';

const TravelBot = () => {
  const [topFiveCountries, setTopFiveCountries] = useState([]);
  const [selectedCountry, setSelectedCountry] = useState(null);

  useEffect(() => {
    const fetchTopFiveCountries = async () => {
      try {
        const response = await fetch('/api/countries/topfive');
        const data = await response.json();
        setTopFiveCountries(data);
      } catch (error) {
        console.error('Error fetching top five countries:', error);
      }
    };

    fetchTopFiveCountries();
  }, []); // Empty dependency array ensures the effect runs once on mount

  const handleCountryClick = async (countryName) => {
    try {
      const response = await fetch(`/api/countries/summary/${countryName}`);
      const data = await response.json();
      setSelectedCountry(data);
    } catch (error) {
      console.error(`Error fetching country summary for ${countryName}:`, error);
    }
  };

  return (
    <div>
      <h2>Travel Bot</h2>
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Capital</th>
            {/* Add more columns as needed */}
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {topFiveCountries.map(country => (
            <tr key={country.name}>
              <td>{country.name}</td>
              <td>{country.capital}</td>
              {/* Add more columns as needed */}
              <td>
                <button onClick={() => handleCountryClick(country.name)}>
                  View Details
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {selectedCountry && (
        <div>
          <h3>{selectedCountry.countryName} Details</h3>
          <p>Capital: {selectedCountry.capital}</p>
          {/* Display more details as needed */}
        </div>
      )}
    </div>
  );
};

export default TravelBot;


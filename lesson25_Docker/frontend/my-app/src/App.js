import React, { useState, useEffect } from 'react';
import './App.css';

function App() {
  // State to hold the result
  const [result, setResult] = useState('');

  // useEffect hook to fetch the data from the backend when the component mounts
  useEffect(() => {
    // Define the async function inside the useEffect
    const fetchData = async () => {
      try {
        // Make the request to the backend API
        const response = await fetch('http://localhost:8081/wf');

        // Check if the response is successful (status code 200-299)
        if (! response.ok) {
          throw new Error(`Error: ${response.statusText}`);
        }

        // Parse the JSON response
        const data = await response.json();

        // Update the result state with the data from the response
          setResult(JSON.stringify(data));
      } catch (error) {
        // Handle any errors
        console.error('Error fetching data:', error);
        setResult('Failed to fetch data');
      }
    };

    // Call the async function to fetch data
    fetchData();
  }, []);  // Empty dependency array means this effect runs only once when the component mounts

  return (
    <div className="App">
          <p>{result}</p>
    </div>
  );
}

export default App;

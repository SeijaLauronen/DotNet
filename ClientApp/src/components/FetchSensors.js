/* LAITETAANKIN SAMAAN KOMPONENTTIIN SENSORIT JA MITTAUKSET*/
/*
import React, { Component } from 'react';

export class FetchSensors extends Component {
  static displayName = FetchSensors.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true }; //TODO ainakin nimeä uudestaan
  }

  componentDidMount() {
    this.populateSensors();
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

    async populateSensors() {
    //const response = await fetch('weatherforecast'); //tässä ei urlia localhost jne, 
    //kun saman sovelluksen sisällä sekä API että Client. 
    //Selain (js) osaa suoraan tuon fetch-kutsun
    
   // const response = await fetch('https://localhost:44324/weatherforecast');//tähän laitettu nyt localhost erikseen
   const response = await fetch('https://localhost:44324/samiapi/sensors');
    const data = await response.json();
    //this.setState({ forecasts: data, loading: false });
    console.log("SAMI:sta sensorit",data); //Sivulla näkyy vain Loading, Katso F12 Console
  }
}
*/
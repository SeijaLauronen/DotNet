//import './App.css';
import React, {Component} from 'react';
import { useState } from 'react';

  class FetchSamiData extends React.Component {
    static displayName = FetchSamiData.name;//Tämä on varmaan se mikä näkyy App.js:n Navlinkissä
    constructor(props) {
      super(props);
      //this.state = {value: ''};

      this.state = {
        value: '',
        forecasts: [],
        measurements:[],
        sensors:[],
        selectedSensor:"4096", 
        loadingsensors: true,
        loadingmeasurements:true,
        loading: true
        };



      this.handleChange = this.handleChange.bind(this);
      this.handleSensorChange= this.handleSensorChange.bind(this);
      this.handleSubmit = this.handleSubmit.bind(this);
    }
  

    componentDidMount() {
      this.populateSensors();         
    }



    handleChange(event) {    this.setState({value: event.target.value});  }
    handleSensorChange(event) {this.setState({selectedSensor: event.target.value});}

    handleSubmit(event) {
      alert('A name was submitted: ' + this.state.value);
      //this.populateMeasurements(this.selectedSensor);//TODO pitäsi saada oikeat arvot
      this.populateMeasurements("4098");
      event.preventDefault();
    }
  
    render() {
      let sensors = this.state.loadingsensors
            ? <p><em>Loading sensors...</em></p>
        : FetchSamiData.renderSensorOptions(this.state.sensors, this.state.selectedSensor);

        let measurementcontents = this.state.loadingmeasurements
      ? <p><em>Loading measurements...</em></p>
      : FetchSamiData.renderMeasurements(this.state.measurements);

      return (
        <div>
        <form onSubmit={this.handleSubmit}>
          
          {sensors}
          <label>
            Name:
            <input type="text" value={this.state.value} onChange={this.handleChange} />        </label>
          <input type="submit" value="Submit" />
        </form>
        {measurementcontents}
        </div>
      );
    }


static  renderSensorOptions(sensors,selectedSensor){

//Täältä ei pääse käsiks tuonne state-juttuihin eikä eventhandlereihin

      return (
        <div>
        <label>Sensori:</label>
        <select>
                  <option value="">Valitse...</option>
                  {sensors.map(sensor =>
                  <option key= {sensor.tag} value={sensor.tag}>{sensor.tag}: {sensor.description}</option>                      
                   )} 
        </select>

        </div>
      );

    }

    static  renderMeasurements(measurements){

      //Täältä ei pääse käsiks tuonne state-juttuihin eikä eventhandlereihin
      
            return (
              <table className='table table-striped' aria-labelledby="tabelLabel">
              <thead>
                  <tr>
                      <th>TAG</th>
                      <th>AIKA</th>
                      
                      <th>arvo</th>
                  </tr>
              </thead>
              <tbody>
                  {measurements.map(measurement =>
                      <tr key={measurement.tag}>
                          <td>{measurement.tag}</td>
                          <td>{measurement.timestampISO8601}</td>                        
                          <td>{measurement.value}</td>                      
                      </tr>
                  )}
              </tbody>
          </table>
            );
      
          }



    async populateSensors() {     
      const response = await fetch('https://localhost:44324/samiapi/sensors'); //käytä tätä ja laita oikea portti, jos ajat Clienttia erikseen
      //const response = await fetch('samiapi/sensors'); //käytä tätä kun ajat clientin ja servicen Visual Studiolla
      const data = await response.json();
      this.setState({ sensors: data, loadingsensors: false });
      console.log("SAMI:sta sensorit", data); //Katso F12 Console tai inspectillä näkyy myös react-extensionilla, kun menee komponentin kohdalle
  }

  async populateMeasurements(id) {     
    const response = await fetch('https://localhost:44324/samiapi/measurements/'+id); //TODO kts syntaksi
    const data = await response.json();
    this.setState({ measurements: data, loadingmeasurements: false });
    console.log("SAMI:sta data id:llä "+id,data); //Sivulla näkyy vain Loading, Katso F12 Console
  }



  }






export default FetchSamiData;

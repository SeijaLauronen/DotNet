import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    
      this.state = {
          forecasts: [],
          measurements:[],
          sensors:[],
          selectedSensor:"4096", 
          loadingsensors: true,
          loadingmeasurements:true,
          loading: true
      };
    this.valitseClicked = this.valitseClicked.bind(this);
    this.handleSelectedClick = this.handleSelectedClick.bind(this);
  }

    componentDidMount() {
      this.populateSensors();
      this.populateMeasurementData();
      //alert(this.selectedSensor);
      //this.populateMData(this.selectedSensor); 
      this.populateMData("4096");     
  }

  //Täältä vinkkiä miten saa tuon id:n välitettyä. Tuo event on ehkä nyt ylimääräinen
    //https://stackoverflow.com/questions/42576198/get-object-data-and-target-element-from-onclick-event-in-react-js
    valitseClicked(event, id) {
      alert("valittu:"+id); 
      this.setState({selectedSensor:id});
      //https://stackoverflow.com/questions/51503687/proper-fetch-call-for-delete-using-json-server              
      //https://stackoverflow.com/questions/45654080/fetch-delete-request-method-using-react-redux-is-not-deleting

      this.populateMData(id);     
  }

  handleSelectedClick(e) {

    alert (e.target.name);
    //this.props.onDeleteClick(e.target.name);
    this.setState({selectedSensor:e.target.name});

}

static renderMeasurementsTable(measurements) {
    return (
       <p>renderMeasurementsTable</p>
     /*
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>TAG</th>
                    <th>AIKA</th>
                    <th>eka sensori</th>
                    <th>arvo</th>
                </tr>
            </thead>
            <tbody>
                {measurements.map(measurement =>
                    <tr key={measurement.tag}>
                        <td>{measurement.tag}</td>
                        <td>{measurement.TimestampISO8601}</td>
                        <td>{measurement.data.tag}</td>
                        <td>{measurement.data.value}</td>                      
                    </tr>
                )}
            </tbody>
        </table>
        */
    );
}



    static renderSensorOptions(sensors) {
      let url = "https://localhost:44324/fetch-data/"
        return (
         /* <p>sensorit haettu. {sensors.length}</p>*/
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Valitse</th>
                        <th>tag</th>
                        <th>date (C)</th>
                        <th>unit (F)</th>
                        <th>desc</th>
                    </tr>
                </thead>
                <tbody>
                    {sensors.map(sensor =>
                        <tr key={sensor.tag}>
                            <td>
                              <a href= {url,sensor.tag}>Ei toimi Valitse {url,sensor.tag}</a>
                              <button value={sensor.tag} onClick={((e) => this.valitseClicked(e, sensor.tag))}>Ei toimi:Valitse </button>
                              <button name={sensor.tag} onClick={this.handleSelectedClick}>Ei toimi: Valitse: {sensor.tag}</button>
                              <button name={sensor.tag} onClick={() => this.setState({ selectedSensor: '4098' }) }> Valitse: {sensor.tag}</button>
                              </td>
                            <td>{sensor.tag}</td>
                            <td>{sensor.date}</td>
                            <td>{sensor.unit}</td>
                            <td>{sensor.description}</td>
                        </tr>
                    )}
                </tbody>
            </table>
            
        );
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

    let sensors = this.state.loadingsensors
            ? <p><em>Loading sensors...</em></p>
        : FetchData.renderSensorOptions(this.state.sensors);
    
    let measurementcontents = this.state.loadingmeasurements
      ? <p><em>Loading measurements...</em></p>
      : FetchData.renderMeasurementsTable(this.state.measurements);

      /*
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.forecasts);
      */

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {sensors}
        {measurementcontents}
      </div>
    );
  }





  async populateMData(id) {
    //alert("populateMData:"+id);
    //const response = await fetch('weatherforecast'); //tässä ei urlia localhost jne, 
    //kun saman sovelluksen sisällä sekä API että Client. 
    //Selain (js) osaa suoraan tuon fetch-kutsun
    
   // const response = await fetch('https://localhost:44324/weatherforecast');//tähän laitettu nyt localhost erikseen
   //const response = await fetch('https://localhost:44324/weatherforecast/sami');
     const response = await fetch('https://localhost:44324/samiapi/measurements/'+id); //TODO kts syntaksi
    const data = await response.json();
    //this.setState({ forecasts: data, loading: false });
    console.log("SAMI:sta data id:llä "+id,data); //Sivulla näkyy vain Loading, Katso F12 Console
    }




    async populateSensors() {
        //const response = await fetch('weatherforecast'); //tässä ei urlia localhost jne, 
        //kun saman sovelluksen sisällä sekä API että Client. 
        //Selain (js) osaa suoraan tuon fetch-kutsun

        // const response = await fetch('https://localhost:44324/weatherforecast');//tähän laitettu nyt localhost erikseen
        const response = await fetch('https://localhost:44324/samiapi/sensors');
        const data = await response.json();
        this.setState({ sensors: data, loadingsensors: false });
        console.log("SAMI:sta sensorit", data); //Sivulla näkyy vain Loading, Katso F12 Console
    }

    async populateMeasurementData() {
      //const response = await fetch('weatherforecast'); //tässä ei urlia localhost jne, 
      //kun saman sovelluksen sisällä sekä API että Client. 
      //Selain (js) osaa suoraan tuon fetch-kutsun
      
     // const response = await fetch('https://localhost:44324/weatherforecast');//tähän laitettu nyt localhost erikseen
     //const response = await fetch('https://localhost:44324/weatherforecast/sami');
      let url='https://localhost:44324/samiapi/measurements/';
      
      //url +=this.selectedSensor;
      
      //const response = await fetch('https://localhost:44324/samiapi/measurements/');
      const response = await fetch(url);
      const data = await response.json();
      this.setState({ measurements: data, loadingmeasurements: false });
      console.log("SAMI:sta data",data); //Sivulla näkyy vain Loading, Katso F12 Console
      }
}

*************
Uusi WEB api projekti, templatena React
Huom:Startup:SPA
KTS package.json
public-kansiossa varsinainen SPA-sovellus
index.js alkusivu
luokkatyylinen react on oletuksena, vaikka se on vanhempi tyyli. Funktionaalinen tyyi olisi uudempi (react paketista 16...xxx lähtien käytössä))
--Kun jonkin kompnentin tila muuttu, se renderöidään uudestaan (ja sen lapsikomponentit)
Jos selaimesta laittaa refresh, kaikki sivut ja komponentit ladataan uusikis, mutta jos klikataan jotain komponentin nappia/urlia,  niin ladataan vain sisältö uusiksi.
VSCodella kannattaa muokata. Avaa siellä se kansio, missä on package.json ja sitten valikosta terminal, new terminal. Anna komento
npm start 

Selaimessa pelkän API:n saa: https://localhost:44324/weatherforecast
CORS: selain estää HTTP-pyynnön (serveri ja API-kutsu eri porteissa)
-> response-header (kts selaimen inspector F12 Network Header)
sovellus tai webbipalvelin voi asettaa sopivat headerit
Koodissa: Enable cross-origin Requests

KTS WeatherForeCastControllerista, kuinka teet JSON datasta luokan!!

graafin piirtoon:
npm i react-charts


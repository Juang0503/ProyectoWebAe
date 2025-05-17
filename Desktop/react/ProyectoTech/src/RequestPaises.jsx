import React, { useEffect, useState } from 'react';
import axios from 'axios';

export default function SelectDesdeLista() {
  const [opciones, setOpciones] = useState([]);
  const [seleccionado, setSeleccionado] = useState('');
  const [graficoSeleccionado, setGraficoSeleccionado] = useState('');
  const [rutaGrafico, setRutaGrafico] = useState('');

  // Mapea el nombre legible con la ruta del endpoint
  const opcionesGraficos = [
    {
      nombre: "Producción de Energía Renovable por Fuente",
      endpoint: "/grafico/barras"
    },
    {
      nombre: "Participación de Energías Renovables",
      endpoint: "/grafico/torta"
    },
    {
      nombre: "Tendencia en la Capacidad Instalada",
      endpoint: "/grafico/lineas"
    },
    {
      nombre: "Comparación Consumo Renovable vs Convencional",
      endpoint: "/grafico/area"
    }
  ];

  useEffect(() => {
    axios.get('http://localhost:8000/paises')
      .then(response => {
        setOpciones(response.data);
        if (response.data.length > 0) {
          setSeleccionado(response.data[0]);
        }
      })
      .catch(error => {
        console.error('Error al obtener los datos:', error);
      });
  }, []);

  useEffect(() => {
    if (graficoSeleccionado) {
      const grafico = opcionesGraficos.find(g => g.nombre === graficoSeleccionado);
      if (grafico) {
        axios.get(`http://localhost:8000${grafico.endpoint}`)
          .then(response => {
            setRutaGrafico(`http://localhost:8000${response.data.ruta}`);
          })
          .catch(error => {
            console.error('Error al obtener el gráfico:', error);
          });
      }
    }
  }, [graficoSeleccionado]);

  return (
    <div>
      <select id="pais" value={seleccionado} onChange={e => setSeleccionado(e.target.value)}>
        {opciones.map((opcion, index) => (
          <option key={index} value={opcion}>{opcion}</option>
        ))}
      </select>

      <div style={{ marginTop: '1rem' }}>
        <p>Selecciona un gráfico:</p>
        {opcionesGraficos.map((grafico) => (
          <button
            key={grafico.nombre}
            onClick={() => setGraficoSeleccionado(grafico.nombre)}
            style={{
              margin: '0.3rem',
              padding: '0.5rem 1rem',
              backgroundColor: graficoSeleccionado === grafico.nombre ? '#4caf50' : '#eee',
              color: graficoSeleccionado === grafico.nombre ? 'white' : 'black',
              border: 'none',
              borderRadius: '4px',
              cursor: 'pointer'
            }}
          >
            {grafico.nombre}
          </button>
        ))}
      </div>

      {rutaGrafico && (
        <div style={{ marginTop: '1rem' }}>
          <img src={rutaGrafico} alt={`Gráfico: ${graficoSeleccionado}`} style={{ maxWidth: '100%', height: 'auto' }} />
        </div>
      )}
    </div>
  );
}

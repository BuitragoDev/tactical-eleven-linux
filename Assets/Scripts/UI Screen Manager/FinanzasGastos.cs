using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class FinanzasGastos
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        int anio;

        private VisualElement root, btnMenos, btnMas;
        private Label tituloVentana, gastoSalarioJugadores, dineroTotal, gastoSalarioEmpleados, salariosJugadores, salariosEmpleados,
                    cancelacionContratatoJugador, cancelacionContratoEmpleado, fichajesJugadores, bonusJugadores, remodelacionGradas,
                    prestamosBancarios, totalGastos;
        private VisualElement[] barrasPositivas;
        private VisualElement[] barrasNegativas;
        private VisualElement[] barraPositivaReal;
        private VisualElement[] barraNegativaReal;
        private Label[] textosValor;
        private Label[] textosMes;

        public FinanzasGastos(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();
            anio = FechaData.temporadaActual;

            // Referencias a objetos de la UI
            btnMenos = root.Q<VisualElement>("btnMenos");
            btnMas = root.Q<VisualElement>("btnMas");

            tituloVentana = root.Q<Label>("lblTituloVentana");
            gastoSalarioJugadores = root.Q<Label>("lblGastoSalarioJugadores");
            dineroTotal = root.Q<Label>("lblDineroTotalClub");
            gastoSalarioEmpleados = root.Q<Label>("lblGastoSalarioEmpleados");
            salariosJugadores = root.Q<Label>("salario-jugadores");
            salariosEmpleados = root.Q<Label>("salario-empleados");
            cancelacionContratatoJugador = root.Q<Label>("cancelacion-contratos-jugadores");
            cancelacionContratoEmpleado = root.Q<Label>("cancelacion-contratos-empleados");
            fichajesJugadores = root.Q<Label>("fichajes");
            bonusJugadores = root.Q<Label>("bonus-jugadores");
            remodelacionGradas = root.Q<Label>("remodelacion-gradas");
            prestamosBancarios = root.Q<Label>("prestamos-bancarios");
            totalGastos = root.Q<Label>("total");

            barrasPositivas = new VisualElement[12];
            barrasNegativas = new VisualElement[12];
            barraPositivaReal = new VisualElement[12];
            barraNegativaReal = new VisualElement[12];
            textosValor = new Label[12];
            textosMes = new Label[12];

            for (int i = 0; i < 12; i++)
            {
                int n = i + 1;

                barrasPositivas[i] = root.Q<VisualElement>($"mes{n}-positivo");
                barrasNegativas[i] = root.Q<VisualElement>($"mes{n}-negativo");
                textosValor[i] = root.Q<Label>($"mes{n}-valor");
                textosMes[i] = root.Q<Label>($"mes{n}-texto");

                // Crear la barra hija dentro del positivo
                var barPos = new VisualElement();
                barPos.style.width = new Length(100, LengthUnit.Percent);
                barPos.style.height = 0;
                barPos.style.backgroundColor = new StyleColor(new Color32(56, 78, 63, 255));
                barPos.style.position = Position.Absolute;
                barPos.style.bottom = 0;

                barrasPositivas[i].Add(barPos);
                barraPositivaReal[i] = barPos;

                // Crear la barra hija dentro del negativo
                var barNeg = new VisualElement();
                barNeg.style.width = new Length(100, LengthUnit.Percent);
                barNeg.style.height = 0;
                barNeg.style.backgroundColor = new StyleColor(new Color32(156, 32, 7, 255));
                barNeg.style.position = Position.Absolute;
                barNeg.style.top = 0;

                barrasNegativas[i].Add(barNeg);
                barraNegativaReal[i] = barNeg;
            }

            ConfigurarTextoTemporada();

            // Carga de Valores
            dineroTotal.text = Constants.CambioDivisa(miEquipo.Presupuesto).ToString("N0") + " " + CargarSimboloMoneda();

            gastoSalarioEmpleados.text = Constants.CambioDivisa(EmpleadoData.SalarioTotalEmpleados(miEquipo.IdEquipo)).ToString("N0") + " " + CargarSimboloMoneda();

            List<Jugador> miPlantilla = JugadorData.ListadoJugadoresCompleto(miEquipo.IdEquipo);
            int gastoSueldosJugadores = 0;
            foreach (Jugador jugador in miPlantilla)
            {
                gastoSueldosJugadores += (int)(jugador.SalarioTemporada ?? 0);
            }
            gastoSalarioJugadores.text = Constants.CambioDivisa(gastoSueldosJugadores).ToString("N0") + " " + CargarSimboloMoneda();

            CargarGrafica(FechaData.temporadaActual);
            CargarCantidades(FechaData.temporadaActual);

            btnMenos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                anio--;
                if (anio == 2025)
                {
                    btnMenos.style.visibility = Visibility.Hidden;
                }
                else
                {
                    btnMenos.style.visibility = Visibility.Visible;
                }

                if (anio < FechaData.temporadaActual)
                {
                    btnMenos.style.visibility = Visibility.Visible;
                }
                else
                {
                    btnMenos.style.visibility = Visibility.Hidden;
                }
                tituloVentana.text = "Temporada " + anio + "/" + (anio + 1);
                CargarGrafica(anio);
                CargarCantidades(anio);
            });

            btnMas.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                btnMas.style.visibility = Visibility.Visible;
                anio++;
                tituloVentana.text = "Temporada " + anio + "/" + (anio + 1);

                if (anio == FechaData.temporadaActual + 1)
                {
                    btnMas.style.visibility = Visibility.Hidden;
                }

                if (anio == FechaData.temporadaActual)
                {
                    btnMas.style.visibility = Visibility.Hidden;
                }

                CargarGrafica(anio);
                CargarCantidades(anio);
            });
        }

        private void CargarGrafica(int temporada)
        {
            List<Finanza> finanzas = FinanzaData.MostrarFinanzasEquipo(temporada);

            double[] ingresos = new double[12];
            double[] gastos = new double[12];

            foreach (var finanza in finanzas)
            {
                int mesCalendario = finanza.Fecha.Month;
                int mesIndex = (mesCalendario - 7 + 12) % 12;

                if (finanza.Tipo == 1)
                    ingresos[mesIndex] += finanza.Cantidad;
                else if (finanza.Tipo == 2)
                    gastos[mesIndex] += finanza.Cantidad;
            }

            for (int i = 0; i < 12; i++)
            {
                double balance = Constants.CambioDivisa(ingresos[i] - gastos[i]);
                double balanceAltura = Math.Max(Constants.CambioDivisa(-30000000), Math.Min(Constants.CambioDivisa(30000000), balance));
                double altura = Math.Abs(balanceAltura / Constants.CambioDivisa(30000000.0)) * 100.0;

                // -------- 1. Mostrar el valor arriba --------
                textosValor[i].text = balance.ToString("N0") + " " + CargarSimboloMoneda();

                // -------- 2. Mes + Año --------
                textosMes[i].text = ObtenerNombreMes(i);

                // -------- 3. Actualizar barras --------
                if (balance >= 0)
                {
                    barraPositivaReal[i].style.height = new Length((float)altura, LengthUnit.Percent);
                    barraNegativaReal[i].style.height = new Length(0, LengthUnit.Percent);
                }
                else
                {
                    barraNegativaReal[i].style.height = new Length((float)altura, LengthUnit.Percent);
                    barraPositivaReal[i].style.height = new Length(0, LengthUnit.Percent);
                }
            }
        }

        private void CargarCantidades(int temporada)
        {
            List<Finanza> listaFinanzas = FinanzaData.MostrarFinanzasEquipo(temporada);
            int salarioJugador = 0;
            int cancelacionJugador = 0;
            int salarioEmpleado = 0;
            int cancelacionEmpleado = 0;
            int obras = 0;
            int bonus = 0;
            int prestamosGasto = 0;
            int total = 0;
            int traspaso = 0;

            foreach (Finanza finanza in listaFinanzas)
            {
                if (finanza.IdConcepto == 11)
                {
                    salarioJugador += (int)finanza.Cantidad;
                }
                if (finanza.IdConcepto == 12)
                {
                    cancelacionJugador += (int)finanza.Cantidad;
                }
                if (finanza.IdConcepto == 13)
                {
                    salarioEmpleado += (int)finanza.Cantidad;
                }
                if (finanza.IdConcepto == 14)
                {
                    cancelacionEmpleado += (int)finanza.Cantidad;
                }
                if (finanza.IdConcepto == 15)
                {
                    obras += (int)finanza.Cantidad;
                }
                if (finanza.IdConcepto == 16)
                {
                    bonus += (int)finanza.Cantidad;
                }
                if (finanza.IdConcepto == 17)
                {
                    prestamosGasto += (int)finanza.Cantidad;
                }
                if (finanza.IdConcepto == 18)
                {
                    traspaso += (int)finanza.Cantidad;
                }
                if (finanza.Tipo == 2) // Tipo 2 = Gastos
                {
                    total += (int)finanza.Cantidad;
                }
            }
            salariosJugadores.text = Constants.CambioDivisa(salarioJugador).ToString("N0") + " " + CargarSimboloMoneda();
            cancelacionContratatoJugador.text = Constants.CambioDivisa(cancelacionJugador).ToString("N0") + " " + CargarSimboloMoneda();
            salariosEmpleados.text = Constants.CambioDivisa(salarioEmpleado).ToString("N0") + " " + CargarSimboloMoneda();
            cancelacionContratoEmpleado.text = Constants.CambioDivisa(cancelacionEmpleado).ToString("N0") + " " + CargarSimboloMoneda();
            remodelacionGradas.text = Constants.CambioDivisa(obras).ToString("N0") + " " + CargarSimboloMoneda();
            bonusJugadores.text = Constants.CambioDivisa(bonus).ToString("N0") + " " + CargarSimboloMoneda();
            prestamosBancarios.text = Constants.CambioDivisa(prestamosGasto).ToString("N0") + " " + CargarSimboloMoneda();
            fichajesJugadores.text = Constants.CambioDivisa(traspaso).ToString("N0") + " " + CargarSimboloMoneda();
            totalGastos.text = Constants.CambioDivisa(total).ToString("N0") + " " + CargarSimboloMoneda();
        }

        private void ConfigurarTextoTemporada()
        {
            tituloVentana.text = "Temporada " + anio + "/" + (anio + 1);

            if (anio == 2025)
            {
                btnMenos.style.visibility = Visibility.Hidden;
            }
            if (anio == FechaData.temporadaActual)
            {
                btnMas.style.visibility = Visibility.Hidden;
            }
        }

        private string CargarSimboloMoneda()
        {
            string currency = PlayerPrefs.GetString("Currency", string.Empty);

            // Elegir símbolo según moneda
            string simbolo = currency switch
            {
                Constants.EURO_NAME => Constants.EURO_SYMBOL,
                Constants.POUND_NAME => Constants.POUND_SYMBOL,
                Constants.DOLLAR_NAME => Constants.DOLLAR_SYMBOL,
                _ => Constants.EURO_SYMBOL // default
            };

            return simbolo;
        }

        private string ObtenerNombreMes(int indice)
        {
            string[] meses = { "JUL " + anio.ToString(),
                       "AGO " + anio.ToString(),
                       "SEP " + anio.ToString(),
                       "OCT " + anio.ToString(),
                       "NOV " + anio.ToString(),
                       "DIC " + anio.ToString(),
                       "ENE " + (anio + 1).ToString(),
                       "FEB " + (anio + 1).ToString(),
                       "MAR " + (anio + 1).ToString(),
                       "ABR " + (anio + 1).ToString(),
                       "MAY " + (anio + 1).ToString(),
                       "JUN " + (anio + 1).ToString() };
            return meses[indice];
        }
    }
}
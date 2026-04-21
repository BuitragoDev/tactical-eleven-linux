using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class FinanzasPrestamos
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        private MainScreen mainScreen;

        int reputacionFinanciero = 0;
        int pagoSemanal;
        int tasa = 0;

        private VisualElement root, valoracionFinancieroContainer, capitalMas, capitalMenos, semanasMas, semanasMenos;
        private Button btnCancelar1, btnCancelar2, btnCancelar3, btnSolicitarPrestamo;
        private Label nombreFinanciero, textoFinanciero, capitalValor, semanasValor, tasaInteres, pagoSemana, totalCapital, totalCapitalRestante,
                      fecha1, capital1, capitalRestante1, semanas1, semanasRestantes1, tasaInteres1, pagoSemanal1,
                      fecha2, capital2, capitalRestante2, semanas2, semanasRestantes2, tasaInteres2, pagoSemanal2,
                      fecha3, capital3, capitalRestante3, semanas3, semanasRestantes3, tasaInteres3, pagoSemanal3;

        public FinanzasPrestamos(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            valoracionFinancieroContainer = root.Q<VisualElement>("valoracionFinanciero-container");
            capitalMas = root.Q<VisualElement>("capital-mas");
            capitalMenos = root.Q<VisualElement>("capital-menos");
            semanasMas = root.Q<VisualElement>("semanas-menos");
            semanasMenos = root.Q<VisualElement>("semanas-mas");
            btnCancelar1 = root.Q<Button>("btnCancelar1");
            btnCancelar2 = root.Q<Button>("btnCancelar2");
            btnCancelar3 = root.Q<Button>("btnCancelar3");
            btnSolicitarPrestamo = root.Q<Button>("btnSolicitarPrestamo");
            nombreFinanciero = root.Q<Label>("nombre-financiero");
            textoFinanciero = root.Q<Label>("texto-financiero");
            capitalValor = root.Q<Label>("capital-valor");
            semanasValor = root.Q<Label>("semanas-valor");
            tasaInteres = root.Q<Label>("tasaInteres-valor");
            pagoSemana = root.Q<Label>("pagoSemana-valor");
            fecha1 = root.Q<Label>("fecha1");
            capital1 = root.Q<Label>("capital1");
            capitalRestante1 = root.Q<Label>("capitalRestante1");
            semanas1 = root.Q<Label>("semanas1");
            semanasRestantes1 = root.Q<Label>("semanasRestantes1");
            tasaInteres1 = root.Q<Label>("tasa1");
            pagoSemanal1 = root.Q<Label>("pagoSemanal1");
            fecha2 = root.Q<Label>("fecha2");
            capital2 = root.Q<Label>("capital2");
            capitalRestante2 = root.Q<Label>("capitalRestante2");
            semanas2 = root.Q<Label>("semanas2");
            semanasRestantes2 = root.Q<Label>("semanasRestantes2");
            tasaInteres2 = root.Q<Label>("tasa2");
            pagoSemanal2 = root.Q<Label>("pagoSemanal2");
            fecha3 = root.Q<Label>("fecha3");
            capital3 = root.Q<Label>("capital3");
            capitalRestante3 = root.Q<Label>("capitalRestante3");
            semanas3 = root.Q<Label>("semanas3");
            semanasRestantes3 = root.Q<Label>("semanasRestantes3");
            tasaInteres3 = root.Q<Label>("tasa3");
            pagoSemanal3 = root.Q<Label>("pagoSemanal3");
            totalCapital = root.Q<Label>("total-capital");
            totalCapitalRestante = root.Q<Label>("total-capitalRestante");

            CargarDatosFinanciero();
            CargarTerminosPrestamo();
            CargarPrestamosActivos();

            capitalMas.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                var culture = CultureInfo.CurrentCulture;

                // Verifica y convierte el texto del TextBox a decimal
                if (decimal.TryParse(capitalValor.text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, culture, out decimal numero))
                {
                    int capital = (int)Math.Round(numero); // Convierte a entero

                    // Limita el rango del número
                    if (capital < 1_000_000 || capital >= 20_000_000)
                    {
                        return;
                    }

                    // Resta 1,000,000 al número
                    capital += 1_000_000;

                    // Asegura que el resultado esté dentro del rango permitido
                    if (capital < 1_000_000)
                    {
                        return;
                    }

                    // Actualiza el TextBox con el número formateado
                    capitalValor.text = capital.ToString("N0", culture);

                    // Convierte el texto del segundo TextBox (semanas) a entero
                    if (int.TryParse(semanasValor.text, out int semanas))
                    {
                        // Llama a calcularPagoSemanal con valores enteros
                        pagoSemana.text = calcularPagoSemanal(capital, semanas).ToString("N0");
                    }
                }
            });

            capitalMenos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                // Usa CultureInfo para manejar números con formato regional
                var culture = CultureInfo.CurrentCulture;

                // Verifica y convierte el texto del TextBox a decimal
                if (decimal.TryParse(capitalValor.text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, culture, out decimal numero))
                {
                    int capital = (int)Math.Round(numero); // Convierte a entero

                    // Limita el rango del número
                    if (capital <= 1_000_000 || capital > 20_000_000)
                    {
                        return;
                    }

                    // Resta 1,000,000 al número
                    capital -= 1_000_000;

                    // Asegura que el resultado esté dentro del rango permitido
                    if (capital < 1_000_000)
                    {
                        return;
                    }

                    // Actualiza el TextBox con el número formateado
                    capitalValor.text = capital.ToString("N0", culture);

                    // Convierte el texto del segundo TextBox (semanas) a entero
                    if (int.TryParse(semanasValor.text, out int semanas))
                    {
                        // Llama a calcularPagoSemanal con valores enteros
                        pagoSemana.text = calcularPagoSemanal(capital, semanas).ToString("N0");
                    }
                }
            });

            semanasMas.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                // Usa CultureInfo para manejar números con formato regional
                var culture = CultureInfo.CurrentCulture;

                // Verifica y convierte el texto del TextBox de semanas
                if (decimal.TryParse(semanasValor.text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, culture, out decimal semanasNumero))
                {
                    // Limita el rango del número
                    if (semanasNumero <= 5 || semanasNumero > 100)
                    {
                        return;
                    }

                    // Resta 5 al número
                    semanasNumero -= 5;

                    // Actualiza el TextBox con el número formateado
                    semanasValor.text = semanasNumero.ToString("N0", culture);

                    // Verifica y convierte el texto del TextBox de capital
                    if (decimal.TryParse(capitalValor.text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, culture, out decimal capitalNumero))
                    {
                        // Llama al método calcularPagoSemanal con ambos valores como enteros
                        int capital = (int)Math.Round(capitalNumero);
                        int semanas = (int)Math.Round(semanasNumero);

                        pagoSemana.text = calcularPagoSemanal(capital, semanas).ToString("N0");
                    }
                }
            });

            semanasMenos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                var culture = CultureInfo.CurrentCulture;

                // Verifica y convierte el texto del TextBox de semanas
                if (decimal.TryParse(semanasValor.text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, culture, out decimal semanasNumero))
                {
                    // Limita el rango del número
                    if (semanasNumero < 5 || semanasNumero >= 100)
                    {
                        return;
                    }

                    // Suma 5 al número
                    semanasNumero += 5;

                    // Actualiza el TextBox con el número formateado
                    semanasValor.text = semanasNumero.ToString("N0", culture);

                    // Verifica y convierte el texto del TextBox de capital
                    if (decimal.TryParse(capitalValor.text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, culture, out decimal capitalNumero))
                    {
                        // Llama al método calcularPagoSemanal con ambos valores como enteros
                        int capital = (int)Math.Round(capitalNumero);
                        int semanas = (int)Math.Round(semanasNumero);

                        pagoSemana.text = calcularPagoSemanal(capital, semanas).ToString("N0");
                    }
                }
            });

            btnSolicitarPrestamo.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                int capital = int.Parse(capitalValor.text.Trim().Replace(".", ""));
                int semanas = int.Parse(semanasValor.text.Trim().Replace(".", ""));

                // Comprobar primer hueco vacío.
                int huecoVacio = 0;  // Valor predeterminado por si no se encuentra ningún hueco

                List<int> ordenVacio = PrestamoData.OrdenPrestamos(miEquipo.IdEquipo);

                // Si no hay registros previos (lista vacía), asignar el orden 1
                if (ordenVacio.Count == 0)
                {
                    huecoVacio = 1;  // El primer préstamo tendrá el orden 1
                }
                else
                {
                    // Comprobamos los posibles valores de orden (1, 2 y 3)
                    for (int i = 1; i <= 3; i++)
                    {
                        // Si el valor no se encuentra en la lista de ordenes existentes
                        if (!ordenVacio.Contains(i))
                        {

                            huecoVacio = i;  // Asignamos el primer hueco vacío encontrado
                            break;  // Salimos del bucle porque ya encontramos el primer hueco
                        }
                    }
                }

                // Càlculo del pago total con intereses
                double intereses = (capital * tasa) / 100;
                int pagoTotal = (int)Math.Round(capital + intereses);

                Prestamo nuevoPrestamo = new Prestamo
                {
                    Orden = huecoVacio,
                    Fecha = FechaData.hoy.ToString("dd/MM/yyyy"),
                    Capital = capital,
                    CapitalRestante = pagoTotal,
                    Semanas = semanas,
                    SemanasRestantes = semanas,
                    TasaInteres = tasa,
                    PagoSemanal = calcularPagoSemanal(capital, semanas),
                    IdEquipo = miEquipo.IdEquipo,
                };

                PrestamoData.AnadirPrestamo(nuevoPrestamo);

                // Creamos el ingreso
                Finanza nuevoIngreso = new Finanza
                {
                    IdEquipo = miEquipo.IdEquipo,
                    Temporada = FechaData.temporadaActual.ToString(),
                    IdConcepto = 10,
                    Tipo = 1,
                    Cantidad = capital,
                    Fecha = FechaData.hoy.Date
                };
                FinanzaData.CrearIngreso(nuevoIngreso);

                // Agregamos la cantidad al presupuesto
                EquipoData.SumarCantidadAPresupuesto(miEquipo.IdEquipo, capital);

                // Creamos el mensaje
                Empleado? financiero = EmpleadoData.ObtenerEmpleadoPorPuesto("Financiero");
                string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;

                Mensaje mensajePrestamo = new Mensaje
                {
                    Fecha = FechaData.hoy,
                    Remitente = financiero != null ? financiero.Nombre : presidente,
                    Asunto = "Préstamo Bancario",
                    Contenido = $"Me complace informarte de que hemos conseguido la aprobación del préstamo solicitado por el club. El banco ha autorizado una inyección económica de {capital.ToString("N0", new CultureInfo("es-ES"))}€, que ya ha sido ingresada en nuestras cuentas.\nEl préstamo deberá ser pagado en {semanas} semanas y tiene un interés del {tasa} %.\n\nEsta operación nos permitirá afrontar con mayor solidez los retos financieros de la temporada. Confío en que sabrás administrar estos fondos con responsabilidad para reforzar el equipo y cumplir los objetivos marcados.",
                    TipoMensaje = "Notificación",
                    IdEquipo = miEquipo.IdEquipo,
                    Leido = false,
                    Icono = 0 // 0 es icono de equipo
                };

                MensajeData.CrearMensaje(mensajePrestamo);

                CargarPrestamosActivos();
            };

            btnCancelar1.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                btnCancelar1.style.visibility = Visibility.Hidden;

                fecha1.text = "-";
                capital1.text = "-";
                capitalRestante1.text = "-";
                semanas1.text = "-";
                semanasRestantes1.text = "-";
                tasaInteres1.text = "-";
                pagoSemanal1.text = "-";

                // Eliminamos el prestamo de la pantalla y hacemos el pago del importe que falta por abonar
                List<Prestamo> prestamosActivos = PrestamoData.MostrarPrestamos(miEquipo.IdEquipo);
                foreach (var prestamo in prestamosActivos)
                {
                    if (prestamo.Orden == 1)
                    {
                        int liquidacion = prestamo.CapitalRestante;
                        Finanza nuevoGasto = new Finanza
                        {
                            IdEquipo = miEquipo.IdEquipo,
                            Temporada = FechaData.temporadaActual.ToString(),
                            IdConcepto = 17,
                            Tipo = 2,
                            Cantidad = liquidacion,
                            Fecha = FechaData.hoy.Date
                        };
                        FinanzaData.CrearGasto(nuevoGasto);

                        // Restar la indemnización al Presupuesto
                        EquipoData.RestarCantidadAPresupuesto(miEquipo.IdEquipo, liquidacion);

                        // Crear mensaje
                        MensajeCancelacionPrestamo(liquidacion);
                    }
                }
                PrestamoData.EliminarPrestamo(miEquipo.IdEquipo, 1);

                CargarPrestamosActivos();
            };

            btnCancelar2.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                btnCancelar2.style.visibility = Visibility.Hidden;

                fecha2.text = "-";
                capital2.text = "-";
                capitalRestante2.text = "-";
                semanas2.text = "-";
                semanasRestantes2.text = "-";
                tasaInteres2.text = "-";
                pagoSemanal2.text = "-";

                // Eliminamos el prestamo de la pantalla y hacemos el pago del importe que falta por abonar
                List<Prestamo> prestamosActivos = PrestamoData.MostrarPrestamos(miEquipo.IdEquipo);
                foreach (var prestamo in prestamosActivos)
                {
                    if (prestamo.Orden == 2)
                    {
                        int liquidacion = prestamo.CapitalRestante;
                        Finanza nuevoGasto = new Finanza
                        {
                            IdEquipo = miEquipo.IdEquipo,
                            Temporada = FechaData.temporadaActual.ToString(),
                            IdConcepto = 17,
                            Tipo = 2,
                            Cantidad = liquidacion,
                            Fecha = FechaData.hoy.Date
                        };
                        FinanzaData.CrearGasto(nuevoGasto);

                        // Restar la indemnización al Presupuesto
                        EquipoData.RestarCantidadAPresupuesto(miEquipo.IdEquipo, liquidacion);

                        // Crear mensaje
                        MensajeCancelacionPrestamo(liquidacion);
                    }
                }
                PrestamoData.EliminarPrestamo(miEquipo.IdEquipo, 2);

                CargarPrestamosActivos();
            };

            btnCancelar3.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                btnCancelar3.style.visibility = Visibility.Hidden;

                fecha3.text = "-";
                capital3.text = "-";
                capitalRestante3.text = "-";
                semanas3.text = "-";
                semanasRestantes3.text = "-";
                tasaInteres3.text = "-";
                pagoSemanal3.text = "-";

                // Eliminamos el prestamo de la pantalla y hacemos el pago del importe que falta por abonar
                List<Prestamo> prestamosActivos = PrestamoData.MostrarPrestamos(miEquipo.IdEquipo);
                foreach (var prestamo in prestamosActivos)
                {
                    if (prestamo.Orden == 3)
                    {
                        int liquidacion = prestamo.CapitalRestante;
                        Finanza nuevoGasto = new Finanza
                        {
                            IdEquipo = miEquipo.IdEquipo,
                            Temporada = FechaData.temporadaActual.ToString(),
                            IdConcepto = 17,
                            Tipo = 2,
                            Cantidad = liquidacion,
                            Fecha = FechaData.hoy.Date
                        };
                        FinanzaData.CrearGasto(nuevoGasto);

                        // Restar la indemnización al Presupuesto
                        EquipoData.RestarCantidadAPresupuesto(miEquipo.IdEquipo, liquidacion);

                        // Crear mensaje
                        MensajeCancelacionPrestamo(liquidacion);
                    }
                }
                PrestamoData.EliminarPrestamo(miEquipo.IdEquipo, 3);

                CargarPrestamosActivos();
            };
        }

        private void CargarPrestamosActivos()
        {
            List<Prestamo> prestamos = PrestamoData.MostrarPrestamos(miEquipo.IdEquipo);

            if (prestamos != null)
            {
                if (prestamos.Count == 3)
                {
                    btnSolicitarPrestamo.style.visibility = Visibility.Hidden;
                }
                else
                {
                    List<Empleado> empleados = EmpleadoData.MostrarListaEmpleadosContratados(miEquipo.IdEquipo);

                    foreach (Empleado empleado in empleados)
                    {
                        if (empleado.Puesto.Equals("Financiero"))
                        {
                            btnSolicitarPrestamo.style.visibility = Visibility.Visible;
                            break;
                        }
                    }

                }

                int capitalTotal = 0;
                int capitalRestanteTotal = 0;
                foreach (Prestamo prestamo in prestamos)
                {
                    if (prestamo.Orden == 1)
                    {
                        fecha1.text = prestamo.Fecha;
                        capital1.text = Constants.CambioDivisa(prestamo.Capital).ToString("N0") + " " + CargarSimboloMoneda();
                        capitalRestante1.text = Constants.CambioDivisa(prestamo.CapitalRestante).ToString("N0") + " " + CargarSimboloMoneda();
                        semanas1.text = prestamo.Semanas.ToString();
                        semanasRestantes1.text = prestamo.SemanasRestantes.ToString();
                        tasaInteres1.text = prestamo.TasaInteres.ToString() + " %";
                        pagoSemanal1.text = Constants.CambioDivisa(prestamo.PagoSemanal).ToString("N0") + " " + CargarSimboloMoneda();
                        btnCancelar1.style.visibility = Visibility.Visible;
                    }

                    if (prestamo.Orden == 2)
                    {
                        fecha2.text = prestamo.Fecha;
                        capital2.text = Constants.CambioDivisa(prestamo.Capital).ToString("N0") + " " + CargarSimboloMoneda();
                        capitalRestante2.text = Constants.CambioDivisa(prestamo.CapitalRestante).ToString("N0") + " " + CargarSimboloMoneda();
                        semanas2.text = prestamo.Semanas.ToString();
                        semanasRestantes2.text = prestamo.SemanasRestantes.ToString();
                        tasaInteres2.text = prestamo.TasaInteres.ToString() + " %";
                        pagoSemanal2.text = Constants.CambioDivisa(prestamo.PagoSemanal).ToString("N0") + " " + CargarSimboloMoneda();
                        btnCancelar2.style.visibility = Visibility.Visible;
                    }

                    if (prestamo.Orden == 3)
                    {
                        fecha3.text = prestamo.Fecha;
                        capital3.text = Constants.CambioDivisa(prestamo.Capital).ToString("N0") + " " + CargarSimboloMoneda();
                        capitalRestante3.text = Constants.CambioDivisa(prestamo.CapitalRestante).ToString("N0") + " " + CargarSimboloMoneda();
                        semanas3.text = prestamo.Semanas.ToString();
                        semanasRestantes3.text = prestamo.SemanasRestantes.ToString();
                        tasaInteres3.text = prestamo.TasaInteres.ToString() + " %";
                        pagoSemanal3.text = Constants.CambioDivisa(prestamo.PagoSemanal).ToString("N0") + " " + CargarSimboloMoneda();
                        btnCancelar3.style.visibility = Visibility.Visible;
                    }

                    capitalTotal += prestamo.Capital;
                    capitalRestanteTotal += prestamo.CapitalRestante;
                }

                totalCapital.text = Constants.CambioDivisa(capitalTotal).ToString("N0") + " " + CargarSimboloMoneda();
                totalCapitalRestante.text = Constants.CambioDivisa(capitalRestanteTotal).ToString("N0") + " " + CargarSimboloMoneda();
            }

            ActualizarPresupuestoMainScreen();
        }

        private void CargarTerminosPrestamo()
        {
            string[] tasaInteresArray = {
                "9 %",
                "7 %",
                "5 %",
                "3 %",
                "1 %"
            };

            if (reputacionFinanciero >= 1 && reputacionFinanciero <= 5)
            {
                tasaInteres.text = tasaInteresArray[reputacionFinanciero - 1];
            }

            int capital = int.Parse(capitalValor.text.Trim().Replace(".", ""));
            int semanas = int.Parse(semanasValor.text.Trim().Replace(".", ""));
            pagoSemana.text = calcularPagoSemanal(capital, semanas).ToString("N0");
        }

        private void CargarDatosFinanciero()
        {
            // Cargar datos de pantalla
            btnSolicitarPrestamo.style.visibility = Visibility.Hidden;
            textoFinanciero.text = "Necesitas contratar un Financiero.";

            List<Empleado> empleados = EmpleadoData.MostrarListaEmpleadosContratados(miEquipo.IdEquipo);

            foreach (Empleado empleado in empleados)
            {
                if (empleado.Puesto.Equals("Financiero"))
                {
                    nombreFinanciero.text = empleado.Nombre;
                    reputacionFinanciero = empleado.Categoria;
                    MostrarEstrellas(reputacionFinanciero, valoracionFinancieroContainer);
                    btnSolicitarPrestamo.style.visibility = Visibility.Visible;

                    int[] valorTasa = {
                        9, 7, 5, 3, 1
                    };

                    if (empleado.Categoria >= 1 && empleado.Categoria <= 5)
                    {
                        tasa = valorTasa[empleado.Categoria - 1];
                    }
                }
            }

            string[] intereses = {
                "Nuestro gestor financiero puede pedir préstamos a un 9% de interés.",
                "Nuestro gestor financiero puede pedir préstamos a un 7% de interés.",
                "Nuestro gestor financiero puede pedir préstamos a un 5% de interés.",
                "Nuestro gestor financiero puede pedir préstamos a un 3% de interés.",
                "Nuestro gestor financiero puede pedir préstamos a un 1% de interés."
            };

            if (reputacionFinanciero >= 1 && reputacionFinanciero <= 5)
            {
                textoFinanciero.text = intereses[reputacionFinanciero - 1];
            }

            string[] tasaInteres = {
                "9 %",
                "7 %",
                "5 %",
                "3 %",
                "1 %"
            };
        }

        private int calcularPagoSemanal(int capital, int semanas)
        {
            List<Empleado> empleados = EmpleadoData.MostrarListaEmpleadosContratados(miEquipo.IdEquipo);
            foreach (Empleado empleado in empleados)
            {
                if (empleado.Puesto.Equals("Financiero"))
                {
                    reputacionFinanciero = empleado.Categoria;
                }
            }

            int[] interes = {
                9,
                7,
                5,
                3,
                1
            };

            if (reputacionFinanciero >= 1 && reputacionFinanciero <= 5)
            {
                tasa = interes[reputacionFinanciero - 1];
            }

            // Càlculo del pago semanal con intereses
            double intereses = (capital * tasa) / 100;
            double pagoTotal = capital + intereses;
            pagoSemanal = (int)Math.Round((double)pagoTotal / semanas);

            return pagoSemanal;
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

        private void MensajeCancelacionPrestamo(int cantidad)
        {
            // Creamos el mensaje
            Empleado? financiero = EmpleadoData.ObtenerEmpleadoPorPuesto("Financiero");
            string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;

            Mensaje mensajePrestamo = new Mensaje
            {
                Fecha = FechaData.hoy,
                Remitente = financiero != null ? financiero.Nombre : presidente,
                Asunto = "Préstamo Bancario",
                Contenido = $"Me alegra comunicarte que hemos saldado por completo el préstamo pendiente de {Constants.CambioDivisa(cantidad).ToString("N0", new CultureInfo("es-ES"))} {CargarSimboloMoneda()} con la entidad bancaria. Gracias a tu gestión económica, hemos podido cancelar anticipadamente la deuda, liberando al club de cargas financieras y fortaleciendo nuestra estabilidad a medio y largo plazo.\n\nEste es un paso importante hacia un futuro más sostenible. Buen trabajo. Continúa así.",
                TipoMensaje = "Notificación",
                IdEquipo = miEquipo.IdEquipo,
                Leido = false,
                Icono = 0 // 0 es icono de equipo
            };

            MensajeData.CrearMensaje(mensajePrestamo);
        }

        private void ActualizarPresupuestoMainScreen()
        {
            // Actualizar Presupuesto en MainScreen
            Equipo equipo = EquipoData.ObtenerDetallesEquipo((int)miManager.IdEquipo);
            float presupuestoConversion = equipo.Presupuesto * Constants.EURO_VALUE;
            string symbol = Constants.EURO_SYMBOL;

            string currency = PlayerPrefs.GetString("Currency", string.Empty);
            if (currency != string.Empty)
            {
                switch (currency)
                {
                    case Constants.EURO_NAME:
                        presupuestoConversion = equipo.Presupuesto * Constants.EURO_VALUE;
                        symbol = Constants.EURO_SYMBOL;
                        break;
                    case Constants.POUND_NAME:
                        presupuestoConversion = equipo.Presupuesto * Constants.POUND_VALUE;
                        symbol = Constants.POUND_SYMBOL;
                        break;
                    case Constants.DOLLAR_NAME:
                        presupuestoConversion = equipo.Presupuesto * Constants.DOLLAR_VALUE;
                        symbol = Constants.DOLLAR_SYMBOL;
                        break;
                    default:
                        presupuestoConversion = equipo.Presupuesto * Constants.EURO_VALUE;
                        symbol = Constants.EURO_SYMBOL;
                        break;
                }
            }

            mainScreen.miPresupuesto.text = $"{presupuestoConversion.ToString("N0")} {symbol}";
        }

        private void MostrarEstrellas(int categoria, VisualElement contenedor)
        {
            contenedor.Clear();

            Sprite estrellaON = Resources.Load<Sprite>("Icons/estrellaOn");
            Sprite estrellaOFF = Resources.Load<Sprite>("Icons/estrellaOff");

            if (estrellaON == null || estrellaOFF == null)
            {
                Debug.LogError("No se pudieron cargar las imágenes de las estrellas");
                return;
            }

            int numeroEstrellas = 0;

            if (categoria == 5)
            {
                numeroEstrellas = 5;
            }
            else if (categoria == 4)
            {
                numeroEstrellas = 4;
            }
            else if (categoria == 3)
            {
                numeroEstrellas = 3;
            }
            else if (categoria == 2)
            {
                numeroEstrellas = 2;
            }
            else if (categoria == 1)
            {
                numeroEstrellas = 1;
            }
            else
            {
                numeroEstrellas = 0;
            }

            for (int i = 0; i < 5; i++)
            {
                Image estrella = new Image
                {
                    image = i < numeroEstrellas ? estrellaON.texture : estrellaOFF.texture,
                    scaleMode = ScaleMode.ScaleToFit,
                    style =
                    {
                        width = 32,
                        height = 32,
                        marginRight = 3
                    }
                };
                contenedor.Add(estrella);
            }

            contenedor.style.flexDirection = FlexDirection.Row;
        }
    }
}
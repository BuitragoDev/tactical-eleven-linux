using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class ClubEmpleados
    {
        private AudioClip clickSFX;
        private MainScreen mainScreen;

        private Equipo miEquipo;
        private Manager miManager;
        List<Empleado>? listaEmpleadosDisponible = null;
        private Dictionary<Label, string> descripcionesEmpleado;


        private VisualElement root, contenedorEstrellas, popupContainer;
        private Button btnContratar1, btnContratar2, btnContratar3, btnContratar4, btnContratar5, btnContratar6, btnContratar7, btnContratar8,
                       btnDespedir1, btnDespedir2, btnDespedir3, btnDespedir4, btnDespedir5, btnDespedir6, btnDespedir7, btnDespedir8,
                       btnFirmar1, btnFirmar2, btnFirmar3, btnFirmar4, btnFirmar5, btnYes, btnCancel;
        private Label popupText;
        private Label lblNombreEmpleadoFicha, salarioEmpleadoFicha, info;
        private Label lblSegundoEntrenadorNombre, lblDelegadoNombre, lblDirectorTecnicoNombre, lblPreparadorFisicoNombre,
                      lblPsicologoNombre, lblFinancieroNombre, lblEquipoMedicoNombre, lblEncargadoCampoNombre,
                      lblSegundoEntrenadorCategoria, lblDelegadoCategoria, lblDirectorTecnicoCategoria, lblPreparadorFisicoCategoria,
                      lblPsicologoCategoria, lblFinancieroCategoria, lblEquipoMedicoCategoria, lblEncargadoCampoCategoria,
                      lblSegundoEntrenadorSalario, lblDelegadoSalario, lblDirectorTecnicoSalario, lblPreparadorFisicoSalario,
                      lblPsicologoSalario, lblFinancieroSalario, lblEquipoMedicoSalario, lblEncargadoCampoSalario;
        private Label lblEncargadoDisponible1Nombre, lblEncargadoDisponible2Nombre, lblEncargadoDisponible3Nombre, lblEncargadoDisponible4Nombre, lblEncargadoDisponible5Nombre,
                      lblEncargadoDisponible1Puesto, lblEncargadoDisponible2Puesto, lblEncargadoDisponible3Puesto, lblEncargadoDisponible4Puesto, lblEncargadoDisponible5Puesto,
                      lblEncargadoDisponible1Categoria, lblEncargadoDisponible2Categoria, lblEncargadoDisponible3Categoria, lblEncargadoDisponible4Categoria, lblEncargadoDisponible5Categoria,
                      lblEncargadoDisponible1Salario, lblEncargadoDisponible2Salario, lblEncargadoDisponible3Salario, lblEncargadoDisponible4Salario, lblEncargadoDisponible5Salario;


        public ClubEmpleados(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
        {
            root = rootInstance;
            this.mainScreen = mainScreen;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            contenedorEstrellas = root.Q<VisualElement>("estrellasContenedor-ficha");

            // Botones Contratar
            btnContratar1 = root.Q<Button>("btnContratar1");
            btnContratar2 = root.Q<Button>("btnContratar2");
            btnContratar3 = root.Q<Button>("btnContratar3");
            btnContratar4 = root.Q<Button>("btnContratar4");
            btnContratar5 = root.Q<Button>("btnContratar5");
            btnContratar6 = root.Q<Button>("btnContratar6");
            btnContratar7 = root.Q<Button>("btnContratar7");
            btnContratar8 = root.Q<Button>("btnContratar8");

            // Botones Despedir
            btnDespedir1 = root.Q<Button>("btnDespedir1");
            btnDespedir2 = root.Q<Button>("btnDespedir2");
            btnDespedir3 = root.Q<Button>("btnDespedir3");
            btnDespedir4 = root.Q<Button>("btnDespedir4");
            btnDespedir5 = root.Q<Button>("btnDespedir5");
            btnDespedir6 = root.Q<Button>("btnDespedir6");
            btnDespedir7 = root.Q<Button>("btnDespedir7");
            btnDespedir8 = root.Q<Button>("btnDespedir8");

            // Botones Firmar
            btnFirmar1 = root.Q<Button>("btnFirmar1");
            btnFirmar2 = root.Q<Button>("btnFirmar2");
            btnFirmar3 = root.Q<Button>("btnFirmar3");
            btnFirmar4 = root.Q<Button>("btnFirmar4");
            btnFirmar5 = root.Q<Button>("btnFirmar5");

            btnFirmar1.style.display = DisplayStyle.None;
            btnFirmar2.style.display = DisplayStyle.None;
            btnFirmar3.style.display = DisplayStyle.None;
            btnFirmar4.style.display = DisplayStyle.None;
            btnFirmar5.style.display = DisplayStyle.None;

            // Labels ficha
            lblNombreEmpleadoFicha = root.Q<Label>("lblNombreEmpleado-ficha");
            salarioEmpleadoFicha = root.Q<Label>("salarioEmpleado-ficha");
            info = root.Q<Label>("info");

            // Nombres empleados
            lblSegundoEntrenadorNombre = root.Q<Label>("lblSegundoEntrenador-nombre");
            lblDelegadoNombre = root.Q<Label>("lblDelegado-nombre");
            lblDirectorTecnicoNombre = root.Q<Label>("lblDirectorTecnico-nombre");
            lblPreparadorFisicoNombre = root.Q<Label>("lblPreparadorFisico-nombre");
            lblPsicologoNombre = root.Q<Label>("lblPsicologo-nombre");
            lblFinancieroNombre = root.Q<Label>("lblFinanciero-nombre");
            lblEquipoMedicoNombre = root.Q<Label>("lblEquipoMedico-nombre");
            lblEncargadoCampoNombre = root.Q<Label>("lblEncargadoCampo-nombre");

            // Categorías empleados
            lblSegundoEntrenadorCategoria = root.Q<Label>("lblSegundoEntrenador-categoria");
            lblDelegadoCategoria = root.Q<Label>("lblDelegado-categoria");
            lblDirectorTecnicoCategoria = root.Q<Label>("lblDirectorTecnico-categoria");
            lblPreparadorFisicoCategoria = root.Q<Label>("lblPreparadorFisico-categoria");
            lblPsicologoCategoria = root.Q<Label>("lblPsicologo-categoria");
            lblFinancieroCategoria = root.Q<Label>("lblFinanciero-categoria");
            lblEquipoMedicoCategoria = root.Q<Label>("lblEquipoMedico-categoria");
            lblEncargadoCampoCategoria = root.Q<Label>("lblEncargadoCampo-categoria");

            // Salarios empleados
            lblSegundoEntrenadorSalario = root.Q<Label>("lblSegundoEntrenador-salario");
            lblDelegadoSalario = root.Q<Label>("lblDelegado-salario");
            lblDirectorTecnicoSalario = root.Q<Label>("lblDirectorTecnico-salario");
            lblPreparadorFisicoSalario = root.Q<Label>("lblPreparadorFisico-salario");
            lblPsicologoSalario = root.Q<Label>("lblPsicologo-salario");
            lblFinancieroSalario = root.Q<Label>("lblFinanciero-salario");
            lblEquipoMedicoSalario = root.Q<Label>("lblEquipoMedico-salario");
            lblEncargadoCampoSalario = root.Q<Label>("lblEncargadoCampo-salario");

            // Encargados disponibles (Nombre)
            lblEncargadoDisponible1Nombre = root.Q<Label>("lblEncargadoDisponible1-nombre");
            lblEncargadoDisponible2Nombre = root.Q<Label>("lblEncargadoDisponible2-nombre");
            lblEncargadoDisponible3Nombre = root.Q<Label>("lblEncargadoDisponible3-nombre");
            lblEncargadoDisponible4Nombre = root.Q<Label>("lblEncargadoDisponible4-nombre");
            lblEncargadoDisponible5Nombre = root.Q<Label>("lblEncargadoDisponible5-nombre");

            // Encargados disponibles (Puesto)
            lblEncargadoDisponible1Puesto = root.Q<Label>("lblEncargadoDisponible1-puesto");
            lblEncargadoDisponible2Puesto = root.Q<Label>("lblEncargadoDisponible2-puesto");
            lblEncargadoDisponible3Puesto = root.Q<Label>("lblEncargadoDisponible3-puesto");
            lblEncargadoDisponible4Puesto = root.Q<Label>("lblEncargadoDisponible4-puesto");
            lblEncargadoDisponible5Puesto = root.Q<Label>("lblEncargadoDisponible5-puesto");

            // Encargados disponibles (Categoría)
            lblEncargadoDisponible1Categoria = root.Q<Label>("lblEncargadoDisponible1-categoria");
            lblEncargadoDisponible2Categoria = root.Q<Label>("lblEncargadoDisponible2-categoria");
            lblEncargadoDisponible3Categoria = root.Q<Label>("lblEncargadoDisponible3-categoria");
            lblEncargadoDisponible4Categoria = root.Q<Label>("lblEncargadoDisponible4-categoria");
            lblEncargadoDisponible5Categoria = root.Q<Label>("lblEncargadoDisponible5-categoria");

            // Encargados disponibles (Salario)
            lblEncargadoDisponible1Salario = root.Q<Label>("lblEncargadoDisponible1-salario");
            lblEncargadoDisponible2Salario = root.Q<Label>("lblEncargadoDisponible2-salario");
            lblEncargadoDisponible3Salario = root.Q<Label>("lblEncargadoDisponible3-salario");
            lblEncargadoDisponible4Salario = root.Q<Label>("lblEncargadoDisponible4-salario");
            lblEncargadoDisponible5Salario = root.Q<Label>("lblEncargadoDisponible5-salario");

            popupContainer = root.Q<VisualElement>("popup-container");
            btnYes = root.Q<Button>("btnYes");
            btnCancel = root.Q<Button>("btnCancel");
            popupText = root.Q<Label>("popup-text");

            descripcionesEmpleado = new Dictionary<Label, string>
            {
                { lblSegundoEntrenadorNombre,  "Es el encargado de entrenar individualmente a tus jugadores, además de dar un bonus al entrenamiento ofensivo y defensivo del equipo. Un entrenador asistente de más nivel permite entrenar más jugadores de forma individual y aumenta la velocidad con la que subirán los parámetros de tus jugadores." },
                { lblDelegadoNombre,           "Es el encargado de analizar al equipo rival antes de un partido, comunicarte cualquier acontecimiento referente a tu equipo o a las competiciones en las que participes." },
                { lblDirectorTecnicoNombre,    "Te permite realizar una búsqueda muy precisa de todos los jugadores que hay en el juego. También es necesario si quieres informes detallados de jugadores que puedan ser interesantes sus fichajes." },
                { lblPreparadorFisicoNombre,   "Es el encargado de entrenar los atributos físicos de tus jugadores, además de dar un bonus al entrenamiento físico del equipo. Un preparador físico de más nivel aumenta la velocidad con la que subirán los parámetros físicos de tus jugadores." },
                { lblPsicologoNombre,          "Ayuda a tus jguadores si estos están bajos de moral debido a situaciones como pueden ser el estado de ánimo debido a los malos resultados del equipo, la confianza con el entrenador debido a la falta de minutos de juego, o la química de vestuario." },
                { lblFinancieroNombre,         "Controla distintas áreas financieras del club como patrocinadores, ofertas de televisión o abonos de la temporada. El financiero siempre te conseguirá los mejores acuerdos dependiendo de su calidad y si lo deseas se encargará de estas áreas financieras del club." },
                { lblEquipoMedicoNombre,       "El equipo médico está cualificado para tratar las lesiones de tus jugadores y hacer sesiones de fisioterapia entre partidos. Si quieres reducir el tiempo de lesión de un jugador o quieres recuperar mejor la condición física de tus jugadores entre partidos, deberás contratar un buen equipo médico." },
                { lblEncargadoCampoNombre,     "El encargado del campo te informará de los presupustos, reformas y semanas restantes de una obra que hayas encargado en el estadio. Dependiendo de su calidad te ayudará a recortar el tiempo de las obras del estadio." }
            };

            // Eventos de los LABELS
            ConfigurarEmpleado(lblSegundoEntrenadorNombre, lblSegundoEntrenadorSalario, descripcionesEmpleado[lblSegundoEntrenadorNombre], contenedorEstrellas);
            ConfigurarEmpleado(lblDelegadoNombre, lblDelegadoSalario, descripcionesEmpleado[lblDelegadoNombre], contenedorEstrellas);
            ConfigurarEmpleado(lblDirectorTecnicoNombre, lblDirectorTecnicoSalario, descripcionesEmpleado[lblDirectorTecnicoNombre], contenedorEstrellas);
            ConfigurarEmpleado(lblPreparadorFisicoNombre, lblPreparadorFisicoSalario, descripcionesEmpleado[lblPreparadorFisicoNombre], contenedorEstrellas);
            ConfigurarEmpleado(lblPsicologoNombre, lblPsicologoSalario, descripcionesEmpleado[lblPsicologoNombre], contenedorEstrellas);
            ConfigurarEmpleado(lblFinancieroNombre, lblFinancieroSalario, descripcionesEmpleado[lblFinancieroNombre], contenedorEstrellas);
            ConfigurarEmpleado(lblEquipoMedicoNombre, lblEquipoMedicoSalario, descripcionesEmpleado[lblEquipoMedicoNombre], contenedorEstrellas);
            ConfigurarEmpleado(lblEncargadoCampoNombre, lblEncargadoCampoSalario, descripcionesEmpleado[lblEncargadoCampoNombre], contenedorEstrellas);

            // Eventos de los botones
            ConfigurarBotonContratar(btnContratar1, 1);
            ConfigurarBotonContratar(btnContratar2, 2);
            ConfigurarBotonContratar(btnContratar3, 3);
            ConfigurarBotonContratar(btnContratar4, 4);
            ConfigurarBotonContratar(btnContratar5, 5);
            ConfigurarBotonContratar(btnContratar6, 6);
            ConfigurarBotonContratar(btnContratar7, 7);
            ConfigurarBotonContratar(btnContratar8, 8);

            ConfigurarBotonFirmar(btnFirmar1, 0);
            ConfigurarBotonFirmar(btnFirmar2, 1);
            ConfigurarBotonFirmar(btnFirmar3, 2);
            ConfigurarBotonFirmar(btnFirmar4, 3);
            ConfigurarBotonFirmar(btnFirmar5, 4);

            ConfigurarBotonDespedir(btnDespedir1, "Segundo Entrenador");
            ConfigurarBotonDespedir(btnDespedir2, "Delegado");
            ConfigurarBotonDespedir(btnDespedir3, "Director Técnico");
            ConfigurarBotonDespedir(btnDespedir4, "Preparador Físico");
            ConfigurarBotonDespedir(btnDespedir5, "Psicólogo");
            ConfigurarBotonDespedir(btnDespedir6, "Financiero");
            ConfigurarBotonDespedir(btnDespedir7, "Equipo Médico");
            ConfigurarBotonDespedir(btnDespedir8, "Encargado Campo");

            CargarEmpleadosContratados();
        }

        private void CargarEmpleadosDisponibles(List<Empleado> listaEmpleadosDisponible)
        {
            // Verificar que la lista tenga al menos 5 elementos
            if (listaEmpleadosDisponible.Count >= 5)
            {
                // Asignar los valores de los empleados a los TextBox
                lblEncargadoDisponible1Nombre.text = listaEmpleadosDisponible[0].Nombre;
                lblEncargadoDisponible2Nombre.text = listaEmpleadosDisponible[1].Nombre;
                lblEncargadoDisponible3Nombre.text = listaEmpleadosDisponible[2].Nombre;
                lblEncargadoDisponible4Nombre.text = listaEmpleadosDisponible[3].Nombre;
                lblEncargadoDisponible5Nombre.text = listaEmpleadosDisponible[4].Nombre;

                lblEncargadoDisponible1Puesto.text = listaEmpleadosDisponible[0].Puesto;
                lblEncargadoDisponible2Puesto.text = listaEmpleadosDisponible[1].Puesto;
                lblEncargadoDisponible3Puesto.text = listaEmpleadosDisponible[2].Puesto;
                lblEncargadoDisponible4Puesto.text = listaEmpleadosDisponible[3].Puesto;
                lblEncargadoDisponible5Puesto.text = listaEmpleadosDisponible[4].Puesto;

                lblEncargadoDisponible1Categoria.text = listaEmpleadosDisponible[0].Categoria.ToString();
                lblEncargadoDisponible2Categoria.text = listaEmpleadosDisponible[1].Categoria.ToString();
                lblEncargadoDisponible3Categoria.text = listaEmpleadosDisponible[2].Categoria.ToString();
                lblEncargadoDisponible4Categoria.text = listaEmpleadosDisponible[3].Categoria.ToString();
                lblEncargadoDisponible5Categoria.text = listaEmpleadosDisponible[4].Categoria.ToString();

                lblEncargadoDisponible1Salario.text = Constants.CambioDivisa(listaEmpleadosDisponible[0].Salario).ToString("N0") + " " + CargarSimboloMoneda();
                lblEncargadoDisponible2Salario.text = Constants.CambioDivisa(listaEmpleadosDisponible[1].Salario).ToString("N0") + " " + CargarSimboloMoneda();
                lblEncargadoDisponible3Salario.text = Constants.CambioDivisa(listaEmpleadosDisponible[2].Salario).ToString("N0") + " " + CargarSimboloMoneda();
                lblEncargadoDisponible4Salario.text = Constants.CambioDivisa(listaEmpleadosDisponible[3].Salario).ToString("N0") + " " + CargarSimboloMoneda();
                lblEncargadoDisponible5Salario.text = Constants.CambioDivisa(listaEmpleadosDisponible[4].Salario).ToString("N0") + " " + CargarSimboloMoneda();

                btnFirmar1.style.display = DisplayStyle.Flex;
                btnFirmar2.style.display = DisplayStyle.Flex;
                btnFirmar3.style.display = DisplayStyle.Flex;
                btnFirmar4.style.display = DisplayStyle.Flex;
                btnFirmar5.style.display = DisplayStyle.Flex;
            }
        }

        private void CargarEmpleadosContratados()
        {
            LimpiarEmpleadosContratados();
            List<Empleado> listaEmpleadosContratados = EmpleadoData.MostrarListaEmpleadosContratados(miEquipo.IdEquipo);

            if (listaEmpleadosContratados != null)
            {
                foreach (Empleado empleado in listaEmpleadosContratados)
                {
                    if (empleado.Puesto.Equals("Segundo Entrenador"))
                    {
                        lblSegundoEntrenadorNombre.text = empleado.Nombre;
                        lblSegundoEntrenadorCategoria.text = empleado.Categoria.ToString();
                        lblSegundoEntrenadorCategoria.style.color = DeterminarColor(empleado.Categoria);
                        lblSegundoEntrenadorSalario.text = Constants.CambioDivisa(empleado.Salario).ToString("N0") + " " + CargarSimboloMoneda();
                        btnContratar1.style.display = DisplayStyle.None;
                        btnDespedir1.style.display = DisplayStyle.Flex;
                    }
                    else if (empleado.Puesto.Equals("Delegado"))
                    {
                        lblDelegadoNombre.text = empleado.Nombre;
                        lblDelegadoCategoria.text = empleado.Categoria.ToString();
                        lblDelegadoCategoria.style.color = DeterminarColor(empleado.Categoria);
                        lblDelegadoSalario.text = Constants.CambioDivisa(empleado.Salario).ToString("N0") + " " + CargarSimboloMoneda();
                        btnContratar2.style.display = DisplayStyle.None;
                        btnDespedir2.style.display = DisplayStyle.Flex;
                    }
                    else if (empleado.Puesto.Equals("Director Técnico"))
                    {
                        lblDirectorTecnicoNombre.text = empleado.Nombre;
                        lblDirectorTecnicoCategoria.text = empleado.Categoria.ToString();
                        lblDirectorTecnicoCategoria.style.color = DeterminarColor(empleado.Categoria);
                        lblDirectorTecnicoSalario.text = Constants.CambioDivisa(empleado.Salario).ToString("N0") + " " + CargarSimboloMoneda();
                        btnContratar3.style.display = DisplayStyle.None;
                        btnDespedir3.style.display = DisplayStyle.Flex;
                    }
                    else if (empleado.Puesto.Equals("Preparador Físico"))
                    {
                        lblPreparadorFisicoNombre.text = empleado.Nombre;
                        lblPreparadorFisicoCategoria.text = empleado.Categoria.ToString();
                        lblPreparadorFisicoCategoria.style.color = DeterminarColor(empleado.Categoria);
                        lblPreparadorFisicoSalario.text = Constants.CambioDivisa(empleado.Salario).ToString("N0") + " " + CargarSimboloMoneda();
                        btnContratar4.style.display = DisplayStyle.None;
                        btnDespedir4.style.display = DisplayStyle.Flex;
                    }
                    else if (empleado.Puesto.Equals("Psicólogo"))
                    {
                        lblPsicologoNombre.text = empleado.Nombre;
                        lblPsicologoCategoria.text = empleado.Categoria.ToString();
                        lblPsicologoCategoria.style.color = DeterminarColor(empleado.Categoria);
                        lblPsicologoSalario.text = Constants.CambioDivisa(empleado.Salario).ToString("N0") + " " + CargarSimboloMoneda();
                        btnContratar5.style.display = DisplayStyle.None;
                        btnDespedir5.style.display = DisplayStyle.Flex;
                    }
                    else if (empleado.Puesto.Equals("Financiero"))
                    {
                        lblFinancieroNombre.text = empleado.Nombre;
                        lblFinancieroCategoria.text = empleado.Categoria.ToString();
                        lblFinancieroCategoria.style.color = DeterminarColor(empleado.Categoria);
                        lblFinancieroSalario.text = Constants.CambioDivisa(empleado.Salario).ToString("N0") + " " + CargarSimboloMoneda();
                        btnContratar6.style.display = DisplayStyle.None;
                        btnDespedir6.style.display = DisplayStyle.Flex;
                    }
                    else if (empleado.Puesto.Equals("Equipo Médico"))
                    {
                        lblEquipoMedicoNombre.text = empleado.Nombre;
                        lblEquipoMedicoCategoria.text = empleado.Categoria.ToString();
                        lblEquipoMedicoCategoria.style.color = DeterminarColor(empleado.Categoria);
                        lblEquipoMedicoSalario.text = Constants.CambioDivisa(empleado.Salario).ToString("N0") + " " + CargarSimboloMoneda();
                        btnContratar7.style.display = DisplayStyle.None;
                        btnDespedir7.style.display = DisplayStyle.Flex;
                    }
                    else if (empleado.Puesto.Equals("Encargado Campo"))
                    {
                        lblEncargadoCampoNombre.text = empleado.Nombre;
                        lblEncargadoCampoCategoria.text = empleado.Categoria.ToString();
                        lblEncargadoCampoCategoria.style.color = DeterminarColor(empleado.Categoria);
                        lblEncargadoCampoSalario.text = Constants.CambioDivisa(empleado.Salario).ToString("N0") + " " + CargarSimboloMoneda();
                        btnContratar8.style.display = DisplayStyle.None;
                        btnDespedir8.style.display = DisplayStyle.Flex;
                    }
                }
            }
        }

        private void LimpiarEmpleadosContratados()
        {
            lblSegundoEntrenadorNombre.text = string.Empty;
            lblSegundoEntrenadorCategoria.text = string.Empty;
            lblSegundoEntrenadorSalario.text = string.Empty;
            btnContratar1.style.display = DisplayStyle.Flex;
            btnDespedir1.style.display = DisplayStyle.None;

            lblDelegadoNombre.text = string.Empty;
            lblDelegadoCategoria.text = string.Empty;
            lblDelegadoSalario.text = string.Empty;
            btnContratar2.style.display = DisplayStyle.Flex;
            btnDespedir2.style.display = DisplayStyle.None;

            lblDirectorTecnicoNombre.text = string.Empty;
            lblDirectorTecnicoCategoria.text = string.Empty;
            lblDirectorTecnicoSalario.text = string.Empty;
            btnContratar3.style.display = DisplayStyle.Flex;
            btnDespedir3.style.display = DisplayStyle.None;

            lblPreparadorFisicoNombre.text = string.Empty;
            lblPreparadorFisicoCategoria.text = string.Empty;
            lblPreparadorFisicoSalario.text = string.Empty;
            btnContratar4.style.display = DisplayStyle.Flex;
            btnDespedir4.style.display = DisplayStyle.None;

            lblPsicologoNombre.text = string.Empty;
            lblPsicologoCategoria.text = string.Empty;
            lblPsicologoSalario.text = string.Empty;
            btnContratar5.style.display = DisplayStyle.Flex;
            btnDespedir5.style.display = DisplayStyle.None;

            lblFinancieroNombre.text = string.Empty;
            lblFinancieroCategoria.text = string.Empty;
            lblFinancieroSalario.text = string.Empty;
            btnContratar6.style.display = DisplayStyle.Flex;
            btnDespedir6.style.display = DisplayStyle.None;

            lblEquipoMedicoNombre.text = string.Empty;
            lblEquipoMedicoCategoria.text = string.Empty;
            lblEquipoMedicoSalario.text = string.Empty;
            btnContratar7.style.display = DisplayStyle.Flex;
            btnDespedir7.style.display = DisplayStyle.None;

            lblEncargadoCampoNombre.text = string.Empty;
            lblEncargadoCampoCategoria.text = string.Empty;
            lblEncargadoCampoSalario.text = string.Empty;
            btnContratar8.style.display = DisplayStyle.Flex;
            btnDespedir8.style.display = DisplayStyle.None;
        }

        private void LimpiarEmpleadosDisponibles()
        {
            lblEncargadoDisponible1Nombre.text = string.Empty;
            lblEncargadoDisponible2Nombre.text = string.Empty;
            lblEncargadoDisponible3Nombre.text = string.Empty;
            lblEncargadoDisponible4Nombre.text = string.Empty;
            lblEncargadoDisponible5Nombre.text = string.Empty;

            lblEncargadoDisponible1Puesto.text = string.Empty;
            lblEncargadoDisponible2Puesto.text = string.Empty;
            lblEncargadoDisponible3Puesto.text = string.Empty;
            lblEncargadoDisponible4Puesto.text = string.Empty;
            lblEncargadoDisponible5Puesto.text = string.Empty;

            lblEncargadoDisponible1Categoria.text = string.Empty;
            lblEncargadoDisponible2Categoria.text = string.Empty;
            lblEncargadoDisponible3Categoria.text = string.Empty;
            lblEncargadoDisponible4Categoria.text = string.Empty;
            lblEncargadoDisponible5Categoria.text = string.Empty;

            lblEncargadoDisponible1Salario.text = string.Empty;
            lblEncargadoDisponible2Salario.text = string.Empty;
            lblEncargadoDisponible3Salario.text = string.Empty;
            lblEncargadoDisponible4Salario.text = string.Empty;
            lblEncargadoDisponible5Salario.text = string.Empty;

            btnFirmar1.style.display = DisplayStyle.None;
            btnFirmar2.style.display = DisplayStyle.None;
            btnFirmar3.style.display = DisplayStyle.None;
            btnFirmar4.style.display = DisplayStyle.None;
            btnFirmar5.style.display = DisplayStyle.None;
        }

        private void LimpiarFichaEmpleado()
        {
            lblNombreEmpleadoFicha.text = string.Empty;
            salarioEmpleadoFicha.text = string.Empty;
            info.text = string.Empty;
            contenedorEstrellas.Clear();
        }

        private void ConfigurarEmpleado(
            Label lblNombre,
            Label lblSalario,
            string descripcion,
            VisualElement contenedorEstrellas)
        {
            lblNombre.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                lblNombreEmpleadoFicha.text = lblNombre.text;
                salarioEmpleadoFicha.text = lblSalario.text;
                info.text = descripcion;

                Empleado? categoria = EmpleadoData.MostrarCategoriaEmpleado(lblNombre.text);
                MostrarEstrellas(categoria!.Categoria, contenedorEstrellas);
            });
        }

        private void ConfigurarBotonContratar(Button boton, int indice)
        {
            boton.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                listaEmpleadosDisponible = EmpleadoData.MostrarListaEmpleadosDisponibles(indice);
                CargarEmpleadosDisponibles(listaEmpleadosDisponible);
                LimpiarFichaEmpleado();
            };
        }

        private void ConfigurarBotonDespedir(Button boton, string puesto)
        {
            boton.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                popupContainer.style.display = DisplayStyle.Flex;
                popupText.text = "¿ Estás seguro de que quieres despedir al " + puesto + "?";

                // Importante: limpiar listeners previos para evitar duplicados
                btnYes.clicked -= OnYesClick;
                btnCancel.clicked -= OnCancelClick;

                void OnYesClick()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    PagarIndemnizacion(puesto);
                    EmpleadoData.DespedirEmpleado(puesto);

                    // Quitar entrenamiento a los jugadores si se despide al Preparador Físico
                    if (boton == btnDespedir4)
                    {

                        List<Jugador> jugadores = JugadorData.ListadoJugadoresCompleto(miEquipo.IdEquipo);
                        foreach (var jugador in jugadores)
                        {
                            JugadorData.EntrenarJugador(jugador.IdJugador, 0);
                        }
                    }

                    CargarEmpleadosContratados();
                    LimpiarEmpleadosDisponibles();
                    LimpiarFichaEmpleado();

                    btnYes.clicked -= OnYesClick;
                    btnCancel.clicked -= OnCancelClick;
                    popupContainer.style.display = DisplayStyle.None;
                }

                void OnCancelClick()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    popupContainer.style.display = DisplayStyle.None;

                    btnYes.clicked -= OnYesClick;
                    btnCancel.clicked -= OnCancelClick;
                }

                btnYes.clicked += OnYesClick;
                btnCancel.clicked += OnCancelClick;
            };
        }

        private void ConfigurarBotonFirmar(Button boton, int indice)
        {
            boton.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                if (listaEmpleadosDisponible != null && listaEmpleadosDisponible.Count > indice)
                {
                    EmpleadoData.FicharEmpleado(
                        miEquipo.IdEquipo,
                        listaEmpleadosDisponible[indice].IdEmpleado
                    );
                }

                CargarEmpleadosContratados();
                LimpiarEmpleadosDisponibles();
                LimpiarFichaEmpleado();
            };
        }

        private Color DeterminarColor(int puntos)
        {
            if (puntos > 70)
                return new Color32(0x1E, 0x72, 0x3C, 0xFF);   // Verde 
            else if (puntos >= 50)
                return new Color32(0xC6, 0x76, 0x17, 0xFF); // Naranja 
            else
                return new Color32(0xA3, 0x1E, 0x1E, 0xFF); // rojo
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

        private void PagarIndemnizacion(string puesto)
        {
            List<Empleado> listaEmpleadosContratados = EmpleadoData.MostrarListaEmpleadosContratados(miEquipo.IdEquipo);
            foreach (var empleado in listaEmpleadosContratados)
            {
                if (empleado.Puesto.Equals(puesto))
                {
                    // Crear Gasto en la tabla finanzas
                    int indemnizacion = empleado.Salario / 2;
                    Finanza nuevoGasto = new Finanza
                    {
                        IdEquipo = miEquipo.IdEquipo,
                        Temporada = FechaData.temporadaActual.ToString(),
                        IdConcepto = 14,
                        Tipo = 2,
                        Cantidad = indemnizacion,
                        Fecha = FechaData.hoy.Date
                    };
                    FinanzaData.CrearGasto(nuevoGasto);

                    // Restar la indemnización al Presupuesto
                    EquipoData.RestarCantidadAPresupuesto(miEquipo.IdEquipo, indemnizacion);

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
    }
}
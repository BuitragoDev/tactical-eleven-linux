using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class EstadioInformacion
    {
        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root, escudoEquipo, interiorEstadio;
        private Label nombreEstadio, aforo;

        public EstadioInformacion(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;

            // Referencias a objetos de la UI
            escudoEquipo = root.Q<VisualElement>("escudo-equipo");
            interiorEstadio = root.Q<VisualElement>("foto-estadio");
            nombreEstadio = root.Q<Label>("nombre-estadio");
            aforo = root.Q<Label>("aforo");

            Sprite escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/{miEquipo.IdEquipo}");
            if (escudoSprite != null)
                escudoEquipo.style.backgroundImage = new StyleBackground(escudoSprite);

            Sprite fotoInteriorSprite = Resources.Load<Sprite>($"Estadios/{miEquipo.IdEquipo}interior");
            if (fotoInteriorSprite != null)
                interiorEstadio.style.backgroundImage = new StyleBackground(fotoInteriorSprite);

            nombreEstadio.text = miEquipo.Estadio;
            aforo.text = $"{miEquipo.Aforo.ToString("N0")} asientos";
        }
    }
}
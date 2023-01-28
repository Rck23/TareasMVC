using AutoMapper;
using TareasMVC.Entidades;
using TareasMVC.Models;

namespace TareasMVC.Servicios
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() {

            //Mappear
            CreateMap<Tarea, TareaDTO>();
        
        }
    }
}

using VendingApi.Models;

namespace WEMMApi.Dtos
{
    public class DependencesDto
    {
        public List<Model> Models { get; set; }
        public List<WorkMode> WorkModes { get; set; }
        public List<Template> Templates { get; set; }
        public List<MachinePlace> MachinePlaces { get; set; }
        public List<UserListDto> Users { get; set; }
        public List<WorkerListDto> Managers { get; set; }
        public List<WorkerListDto> Enginers { get; set; }
        public List<WorkerListDto> Technics { get; set; }
        public List<ServicePriority> ServicePriorities { get; set; }
    }
}

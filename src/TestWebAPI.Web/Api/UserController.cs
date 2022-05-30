using TestWebAPI.Web.Api;

namespace TestWebAPI.Web.Controllers {
    public class UserController : BaseApiController {

        //private readonly IRepository<User> _repository;
        //private readonly ITokenService _tokenService;

        //public UserController(IRepository<User> repository, I) {
        //    _repository = repository;
        //}

        //// GET: api/Users
        //[HttpGet]
        //public async Task<IActionResult> List() {

        //    var userDTOs = (await _repository.ListAsync())
        //        .Select(user => new UserDTO(
        //            name: user.Name,
        //            gender: user.Gender,
        //            birhday: user.Birthday,
        //            createdBy: user.CreatedBy,
        //            modifiedBy: user.ModifiedBy,
        //            revokedBy: user.RevokedBy
        //        ))
        //        .ToList();

        //    return Ok(userDTOs);
        //}

        //// GET: api/Projects
        //[HttpGet("{id:int}")]
        //public async Task<IActionResult> GetById(int id) {
        //    var projectSpec = new ProjectByIdWithItemsSpec(id);
        //    var project = await _repository.GetBySpecAsync(projectSpec);
        //    if (project == null) {
        //        return NotFound();
        //    }

        //    var result = new ProjectDTO
        //    (
        //        id: project.Id,
        //        name: project.Name,
        //        items: new List<ToDoItemDTO>
        //        (
        //            project.Items.Select(i => ToDoItemDTO.FromToDoItem(i)).ToList()
        //        )
        //    );

        //    return Ok(result);
        //}

        //// POST: api/Projects
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] CreateProjectDTO request) {
        //    var newProject = new Project(request.Name);

        //    var createdProject = await _repository.AddAsync(newProject);

        //    var result = new ProjectDTO
        //    (
        //        id: createdProject.Id,
        //        name: createdProject.Name
        //    );
        //    return Ok(result);
        //}

        //// PATCH: api/Projects/{projectId}/complete/{itemId}
        //[HttpPatch("{projectId:int}/complete/{itemId}")]
        //public async Task<IActionResult> Complete(int projectId, int itemId) {
        //    var projectSpec = new ProjectByIdWithItemsSpec(projectId);
        //    var project = await _repository.GetBySpecAsync(projectSpec);
        //    if (project == null) return NotFound("No such project");

        //    var toDoItem = project.Items.FirstOrDefault(item => item.Id == itemId);
        //    if (toDoItem == null) return NotFound("No such item.");

        //    toDoItem.MarkComplete();
        //    await _repository.UpdateAsync(project);

        //    return Ok();
        //}
    }
}

using ElevenNote.Models.Note;
using ElevenNote.Models.Responses;
using ElevenNote.Services.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // POST api/Note
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreate request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _noteService.CreateNoteAsync(request);
            if (response is not null)
                return Ok(response);

            return BadRequest(new TextResponse("Could not create note."));
        }

        // GET api/Note
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NoteListItem>), 200)]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        }

        // GET api/Note/5
        [HttpGet("{noteId:int}")]
        [ProducesResponseType(typeof(IEnumerable<NoteListItem>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetNoteById([FromRoute] int noteId)
        {
            NoteDetail? detail = await _noteService.GetNoteByIdAsync(noteId);

            // Similar to our service method, we're using a ternary to determine our return type
            // If the returned value (deail is not null, we'll return it insdie a 200 OK
            // Otherwise we'll return a NotFound() 404 response
            return detail is not null
                ? Ok(detail)
                : NotFound();
        }

        // PUT api/Note
        [HttpPut]
        public async Task<IActionResult> UpdateNoteById([FromBody] NoteUpdate request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await _noteService.UpdateNoteAsync(request)
                ? Ok("Note updated successfully.")
                : BadRequest("Note could not be updated.");
        }

        // DELETE api/Note/5
        [HttpDelete("{noteId:int}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int noteId)
        {
            return await _noteService.DeleteNoteAsync(noteId)
                ? Ok($"Note {noteId} was deleted successfully.")
                : BadRequest($"Note {noteId} could not be deleted.");
        }
    }

}

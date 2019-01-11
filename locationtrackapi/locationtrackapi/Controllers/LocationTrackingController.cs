using System;
using Microsoft.AspNetCore.Mvc;

namespace locationtrackapi.Controllers
{
    public class LocationTrackingController:ControllerBase
    {
        private ILoggerManager _logger;
        //private IRepository _repository;
        // public LocationTrackingController(ILoggerManager logger,IRepository repository)
        // {
        //      _logger = logger;
        //     _repository = repository;
        // }

         [HttpGet]
        public IActionResult LocationTracking()
        {         
            try
        {
            // if (owner.IsObjectNull())
            // {
            //     return BadRequest("Owner object is null");
            // }
    
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest("Invalid model object");
            // }
    
            // _repository.Owner.CreateOwner(owner);
    
            // return CreatedAtRoute("OwnerById", new { id = owner.Id }, owner);
            return CreatedAtRoute("LocationTracking", new string[] { "value1", "value2" });
        }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside the LocationTracking action: {ex}");
                return StatusCode(500, "Internal server error");
            }   
        }
 
        // [HttpGet("{id}", Name = "LocationTrackingById")]
        // public IActionResult LocationTrackingById(int id)
        // {           
        // }
 
        // [HttpGet("{id}/location")]
        // public IActionResult LocationTrackingWithDetails(int id)
        // {
        // }
 
        // [HttpPost]
        // public IActionResult CreateLocationTracking([FromBody]userLocationLog locationLog)
        // {        
        // }
 
        // [HttpPut("{id}")]
        // public IActionResult UpdateLocationTracking(int id, [FromBody]userLocationLog locationLog)
        // {          
        // }
 
        // [HttpDelete("{id}")]
        // public IActionResult DeleteLocationTracking(int id)
        // {         
        // }
    }
}
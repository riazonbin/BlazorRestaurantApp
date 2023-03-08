using MudBlazor.Extensions;

namespace BlazorRestaurantApp.Services
{
    public class TimerService
    {
        private PeriodicTimer _timer = new(TimeSpan.FromSeconds(10));
        public Action Elapsed;


        public async Task ExecuteAsync()
        {
            while(await _timer.WaitForNextTickAsync())
            {
                Elapsed.Invoke();
            }
        }

        public TimerService() 
        {
            ExecuteAsync();
        }
    }
}

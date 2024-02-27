
function updateGoalBlock(){
    var dayGoal = localStorage.getItem('day_goal');
    var complete = localStorage.getItem('complete_time') || 0;
    
    var goalValueElement = document.getElementById('dayGoalValue');
    var completeValueElement = document.getElementById('completeValue');
    
    goalValueElement.textContent = dayGoal/60 + ' hours';
    completeValueElement.textContent = complete/3600 + ' hours';
    
    var completionPercentage = ((complete/60) / dayGoal) * 100;
    
    document.getElementById('progressBar').style.setProperty('--progress-width', completionPercentage + '%');
}
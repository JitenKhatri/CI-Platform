﻿using CI_Platform.Models;
using CI_Platform.Models.InputModels;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository.IRepository
{
   public interface IMissionRepository : IRepository<Mission>
    {
        (List<MissionViewModel>, int) GetAllMission(int page , int pageSize);
        (List<MissionViewModel>, int) GetFilteredMissions(MissionInputModel model);
        List<City> GetCitiesForCountry(long countryid);

        List<MissionSkill> MissionSkillList(int id);
        VolunteeringMissionVM GetMissionById(int id, long user_id);
        IEnumerable<CommentViewModel> comment(long user_id, long mission_id, string comment);
        bool add_to_favourite(long user_id, long mission_id);

        bool Rate_mission(long user_id, long mission_id, int rating);
        bool apply_for_mission(long user_id, long mission_id);
        bool Recommend(long user_id, long mission_id, List<long> co_workers);
        VolunteeringMissionVM Next_Volunteers(int count, long user_id, long mission_id);

        TimesheetViewModel Get_Mission_For_TimeSheet(long user_id);

        Timesheet AddTimeSheet(long user_id, TimesheetViewModel model, string type);
        Timesheet EditTimeSheet(long timesheet_id, Models.ViewModels.TimesheetViewModel model, string type);
        bool DeleteTimesheet(long timesheet_id);

    }
}

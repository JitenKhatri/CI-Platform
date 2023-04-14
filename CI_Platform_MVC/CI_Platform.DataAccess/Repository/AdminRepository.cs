﻿using CI_Platform.Areas.Admin.ViewModels;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly CiPlatformContext _db;
        public AdminRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public CrudViewModel GetAllUsers()
        {
            var users = _db.Users.ToList();
            return new CrudViewModel
            {
                Users = users
            };
        }
        public CrudViewModel GetAllMissions()
        {
            var missions = _db.Missions.ToList();
            return new CrudViewModel
            {
                Missions = missions
            };
        }

        public CrudViewModel GetAllThemes()
        {
            var missionThemes = _db.MissionThemes.ToList();
            return new CrudViewModel
            {
                MissionThemes = missionThemes
            };
        }
        
        public MissionTheme AddTheme(string ThemeName, int status)
        {
            MissionTheme NewTheme = new MissionTheme
            {
                Title = ThemeName,
                Status = status
            };
            _db.MissionThemes.Add(NewTheme);
            Save();
            return NewTheme;
        }

        public bool DeleteTheme(int theme_id)
        {
            MissionTheme deletetheme = _db.MissionThemes.FirstOrDefault(t => t.MissionThemeId == theme_id );
            if (deletetheme is not null)
            {
                _db.MissionThemes.Remove(deletetheme);
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public MissionTheme EditTheme(int theme_id,int Status,string ThemeName)
        {
            MissionTheme edittheme = _db.MissionThemes.FirstOrDefault(t => t.MissionThemeId == theme_id);
            if(edittheme is not null)
            {
                edittheme.Status = Status;
                edittheme.Title = ThemeName;
                Save();
                return edittheme;
            }
            else
            {
                return null;
            }
        }
        public CrudViewModel GetAllSkills()
        {
            List<Skill> Skills = _db.Skills.ToList();
            return new CrudViewModel
            {
                Skills = Skills
            };
        }

        public Skill AddSkill(string SkillName,int Status)
        {
            Skill NewSkill = new Skill
            {
                SkillName = SkillName,
                Status = (byte)Status
            };
            _db.Skills.Add(NewSkill);
            Save();
            return NewSkill;
        }
        public Skill EditSkill(int skillid, int Status, string SkillName)
        {
            Skill editskill = _db.Skills.FirstOrDefault(t => t.SkillId == skillid);
            if (editskill is not null)
            {
                editskill.Status = (byte)Status;
                editskill.SkillName = SkillName;
                Save();
                return editskill;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteSkill(int skill_id)
        {
            Skill deleteskilll = _db.Skills.FirstOrDefault(t => t.SkillId == skill_id);
            if (deleteskilll is not null)
            {
                _db.Skills.Remove(deleteskilll);
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public CrudViewModel GetAllStories()
        {
            var stories = _db.Stories.Where(s => s.Status != "DRAFT")
                            .Select(s => new Story
                            {
                                StoryId = s.StoryId,
                                Title = s.Title,
                                Status = s.Status,
                                // Include related Mission entity
                                Mission = _db.Missions.FirstOrDefault(m => m.MissionId == s.MissionId),
                                // Include related User entity
                                User = _db.Users.FirstOrDefault(u => u.UserId == s.UserId)
                            })
                            .ToList();

            return new CrudViewModel
            {
                Stories = stories
            };
        }

        public CrudViewModel GetAllMissionApplications()
        {
            var missionapplications = _db.MissionApplications.Select(ma => new MissionApplication
            {
                MissionApplicationId = ma.MissionApplicationId,
                AppliedAt = ma.AppliedAt,
                ApprovalStatus = ma.ApprovalStatus,
                Mission = _db.Missions.FirstOrDefault(m => m.MissionId == ma.MissionId),
                User = _db.Users.FirstOrDefault(m => m.UserId == ma.UserId)
            }).ToList();
            return new CrudViewModel
            {
                MissionApplications = missionapplications
            };
        }

        public bool ApproveMissionApplication(int MissionApplicationId)
        {
            MissionApplication missionapplication = _db.MissionApplications.FirstOrDefault(ma => ma.MissionApplicationId == MissionApplicationId);
            if(missionapplication != null)
            {
                missionapplication.ApprovalStatus = "APPROVE";
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeclineMissionApplication(int MissionApplicationId)
        {
            MissionApplication missionapplication = _db.MissionApplications.FirstOrDefault(ma => ma.MissionApplicationId == MissionApplicationId);
            if (missionapplication != null)
            {
                missionapplication.ApprovalStatus = "DECLINE";
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PublishStory(int StoryId)
        {
            Story story = _db.Stories.FirstOrDefault(s => s.StoryId == StoryId);
            if(story != null)
            {
                story.Status = "PUBLISHED";
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeclineStory(int StoryId)
        {
            Story story = _db.Stories.FirstOrDefault(s => s.StoryId == StoryId);
            if (story != null)
            {
                story.Status = "DECLINED";
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

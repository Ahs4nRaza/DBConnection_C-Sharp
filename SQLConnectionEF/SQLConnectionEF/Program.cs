using System;
using System.Collections.Generic;
using System.Linq;
using SQLConnectionEF;

class Program
{
    static void Main(string[] args)
    {
        using var context = new AppDbContext();

        SeedData(context);  // Create leagues, teams, and relationships
        DisplayLeaguesWithTeams(context);  // Read and display


        if (context.Teams.Any(t => t.Name == "Real Madrid"))
        {
            UpdateTeamName(context, "Real Madrid", "Real Madrid CF");
        }
        if (context.Leagues.Any(t => t.Name == "La Liga"))
        {
            UpdateLeagueName(context, "La Liga", "Spanish La Liga");
        }

       
        DisplayLeaguesWithTeams(context);

        if (context.Teams.Any(t => t.Name == "Arsenal"))
        {
            DeleteTeam(context, "Arsenal");
        }
        if (context.Leagues.Any(t => t.Name == "UEFA Champions League"))
        {
            DeleteLeague(context, "UEFA Champions League");
        }
        DisplayLeaguesWithTeams(context);
    }

    static void SeedData(AppDbContext context)
    {
        if (context.Leagues.Any() || context.Teams.Any())
        {
            Console.WriteLine("Data already seeded.");
            return;
        }

        var laLiga = new League { Name = "La Liga" };
        var premierLeague = new League { Name = "Premier League" };
        var ucl = new League { Name = "UEFA Champions League" };

        var teams = new List<Team>
        {
            new Team { Name = "Real Madrid" },
            new Team { Name = "Barcelona" },
            new Team { Name = "Atletico Madrid" },
            new Team { Name = "Arsenal" },
            new Team { Name = "Manchester City" },
            new Team { Name = "Liverpool" }
        };

        context.Leagues.AddRange(laLiga, premierLeague, ucl);
        context.Teams.AddRange(teams);
        context.SaveChanges();

        AddTeamsToLeague(context, laLiga, teams[0], teams[1], teams[2]);
        AddTeamsToLeague(context, premierLeague, teams[3], teams[4], teams[5]);
        AddTeamsToLeague(context, ucl, teams.ToArray());

        Console.WriteLine("Data seeded successfully.");
    }

    
    static void AddTeamsToLeague(AppDbContext context, League league, params Team[] teams)
    {
        var leagueTeams = teams.Select(team => new LeagueTeam
        {
            LeagueId = league.LeagueId,
            TeamId = team.TeamId
        }).ToList();

        context.LeagueTeams.AddRange(leagueTeams);
        context.SaveChanges();
    }

   
    static void DisplayLeaguesWithTeams(AppDbContext context)
    {
        Console.WriteLine("Fetching leagues and their teams:");

        var leaguesWithTeams = context.Leagues
            .Select(l => new
            {
                LeagueName = l.Name,
                Teams = l.LeagueTeams.Select(lt => lt.Team.Name).ToList()
            })
            .ToList();

        foreach (var league in leaguesWithTeams)
        {
            Console.WriteLine($"- {league.LeagueName}:");
            foreach (var teamName in league.Teams)
            {
                Console.WriteLine($"   • {teamName}");
            }
        }
    }


    static void UpdateTeamName(AppDbContext context, string oldName, string newName)
    {
        var team = context.Teams.FirstOrDefault(t => t.Name == oldName);
        if (team == null)
        {
            Console.WriteLine($"Team '{oldName}' not found.");
            return;
        }

        team.Name = newName;
        context.SaveChanges();
        Console.WriteLine($"Team name updated from '{oldName}' to '{newName}'.");
    }

  
    static void UpdateLeagueName(AppDbContext context, string oldName, string newName)
    {
        var league = context.Leagues.FirstOrDefault(l => l.Name == oldName);
        if (league == null)
        {
            Console.WriteLine($"League '{oldName}' not found.");
            return;
        }

        league.Name = newName;
        context.SaveChanges();
        Console.WriteLine($"League name updated from '{oldName}' to '{newName}'.");
    }

   
    static void DeleteTeam(AppDbContext context, string teamName)
    {
        var team = context.Teams.FirstOrDefault(t => t.Name == teamName);
        if (team == null)
        {
            Console.WriteLine($"Team '{teamName}' not found.");
            return;
        }

        // Remove associated LeagueTeam entries first
        var leagueTeams = context.LeagueTeams.Where(lt => lt.TeamId == team.TeamId).ToList();
        context.LeagueTeams.RemoveRange(leagueTeams);

        // Remove team
        context.Teams.Remove(team);
        context.SaveChanges();
        Console.WriteLine($"Team '{teamName}' deleted.");
    }

  
    static void DeleteLeague(AppDbContext context, string leagueName)
    {
        var league = context.Leagues.FirstOrDefault(l => l.Name == leagueName);
        if (league == null)
        {
            Console.WriteLine($"League '{leagueName}' not found.");
            return;
        }

        // Remove associated LeagueTeam entries first
        var leagueTeams = context.LeagueTeams.Where(lt => lt.LeagueId == league.LeagueId).ToList();
        context.LeagueTeams.RemoveRange(leagueTeams);

        // Remove league
        context.Leagues.Remove(league);
        context.SaveChanges();
        Console.WriteLine($"League '{leagueName}'deleted.");
    }
}

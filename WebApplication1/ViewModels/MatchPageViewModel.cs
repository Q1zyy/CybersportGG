﻿using WebApplication1.Models;
namespace WebApplication1.ViewModels
{
	public class MatchPageViewModel
	{

		public Event EventName { get; set; }
		
		public Team Team1 { get; set; }
		
		public Team Team2 { get; set; }

		public DateTime Date { get; set; }
		
	}
}

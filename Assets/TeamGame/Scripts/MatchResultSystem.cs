using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MatchResultSystem : MonoBehaviour
{
    public static MatchResultSystem Instance;
    
    [System.Serializable]
    public class MatchOutcome
    {
        public string fullDescription;
        public MatchOutcome(string desc)
        {
            fullDescription = desc;
        }
    }

    public List<MatchOutcome> outcomes = new List<MatchOutcome>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeOutcomes();
    }

    private void InitializeOutcomes()
    {
        if (outcomes.Count > 0) return;

        outcomes.Add(new MatchOutcome("Your team dominated possession and used quick passing and sharp counterattacks to break through the defense. A well-executed team effort led to a solid 3–1 win."));
        outcomes.Add(new MatchOutcome("The match ended in a tense draw. Both goalkeepers pulled off incredible saves, and despite multiple chances, neither side could find the winning goal in the final minutes."));
        outcomes.Add(new MatchOutcome("Tactical mistakes in defense cost the team dearly. The opponent took advantage of poor marking and weak positioning, scoring twice in the second half to take the win."));
        outcomes.Add(new MatchOutcome("Victory snatched at the last moment! Your rookie midfielder delivered a brilliant assist in the 89th minute, securing a dramatic 2–1 win for your side."));
        outcomes.Add(new MatchOutcome("Fatigue showed in the final stretch. Your players struggled to keep up with the opponent’s pace and conceded two late goals. End result: a disappointing 1–3 loss."));
        outcomes.Add(new MatchOutcome("A convincing win thanks to a strong midfield performance. Your players controlled the tempo and distribution, leaving the opposition with few chances to respond."));
        outcomes.Add(new MatchOutcome("Back-to-back matches took a toll. Your team looked sluggish and disorganized, resulting in a painful 0–2 defeat with almost no real chances on goal."));
        outcomes.Add(new MatchOutcome("Substitution magic! A fresh striker came on in the 85th minute and scored a powerful header from a corner. The team won 2–1 in dramatic fashion."));
        outcomes.Add(new MatchOutcome("The team missed a penalty early in the game and never recovered. Despite pushing forward, defensive lapses led to a 1–2 defeat against a weaker opponent."));
        outcomes.Add(new MatchOutcome("The captain rallied the team after going 0–1 down early. With a key interception and a long-range goal, he led the comeback to a 1–1 draw."));
        outcomes.Add(new MatchOutcome("Incredible upset! Your team beat the league leaders 2–0 with disciplined pressing and precise finishing. Fans and pundits are still in shock!"));
        outcomes.Add(new MatchOutcome("Your goalkeeper was in top form, making three crucial saves in the second half. His performance earned him the man of the match in a hard-fought 1–0 win."));
        outcomes.Add(new MatchOutcome("The opposition's relentless high press caused constant turnovers. Your defense collapsed under pressure, and the match ended with a heavy 0–3 defeat."));
        outcomes.Add(new MatchOutcome("After an early lead, your team switched to a defensive approach. It worked for most of the game until a late equalizer resulted in a 1–1 draw."));
        outcomes.Add(new MatchOutcome("Rain made the pitch slippery, and your team struggled with ball control. Several good chances were wasted, and the match ended in a frustrating 0–0 draw."));
        outcomes.Add(new MatchOutcome("Best match of the season! Flawless positioning, fast transitions, and confident finishing led to a dominant 4–1 victory and huge morale boost."));
        outcomes.Add(new MatchOutcome("The coach's strategy backfired. A risky offensive setup left gaps at the back, which the opponents exploited. A preventable 1–3 loss."));
        outcomes.Add(new MatchOutcome("Your team started on fire, scoring two goals within 15 minutes. They held their lead comfortably, showing maturity and discipline in a 2–0 win."));
        outcomes.Add(new MatchOutcome("Controversy ruled the match. A clear goal was disallowed for offside, leading to frustration and a chaotic finish. Final score: 0–1 loss."));
        outcomes.Add(new MatchOutcome("The match was physical and rough. Your players took several knocks but stayed focused, defending well to hold onto a hard-earned 0–0 draw."));
    }

    public List<string> GenerateRandomMatchResult()
    {
        int index = Random.Range(0, outcomes.Count);
        MatchOutcome selected = outcomes[index];

        int skillPoints = Random.Range(1, 6);
        int ratingChange = Random.Range(-50, 51);

        WalletController.Instance.skillPoints += skillPoints;
        WalletController.Instance.rating += ratingChange;

        List<string> strings = new List<string>();
        strings.Add(skillPoints.ToString());
        strings.Add(ratingChange.ToString());
        strings.Add(selected.fullDescription);

        return strings;
    }
}

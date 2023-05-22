# Script used to test team/player order logic
teams = 4
players = 4
round = 0

for turn in range(teams * 4):

    nextTeam = turn % teams

    if (nextTeam == 0):
        round += 1

    nextPlayer = (round - 1) % players

    print("Next Team: ", nextTeam, " Player: ", nextPlayer)

input("Press enter to exit..")
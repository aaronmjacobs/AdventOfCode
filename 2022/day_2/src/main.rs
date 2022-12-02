#[derive(Copy, Clone, PartialEq)]
enum Shape {
    Rock,
    Paper,
    Scissors
}

enum Outcome {
    Lose,
    Draw,
    Win
}

fn int_to_shape(i: i32) -> Shape {
    return match i {
        0 => Shape::Rock,
        1 => Shape::Paper,
        2 => Shape::Scissors,
        _ => panic!("Invalid shape value"),
    };
}

fn parse_shape(c: char) -> Shape {
    return match c {
        'A' | 'X' => Shape::Rock,
        'B' | 'Y' => Shape::Paper,
        'C' | 'Z' => Shape::Scissors,
        _ => panic!("Invalid shape input"),
    };
}

fn parse_outcome(c: char) -> Outcome {
    return match c {
        'X' => Outcome::Lose,
        'Y' => Outcome::Draw,
        'Z' => Outcome::Win,
        _ => panic!("Invalid outcome input"),
    };
}

fn get_winning_shape(shape: Shape) -> Shape {
    let shape_int: i32 = shape as i32;
    return int_to_shape((shape_int + 1) % 3);
}

fn get_losing_shape(shape: Shape) -> Shape {
    let shape_int: i32 = shape as i32;
    return int_to_shape((shape_int + 2) % 3);
}

fn score_move(my_move: Shape) -> i32 {
    return match my_move {
        Shape::Rock => 1,
        Shape::Paper => 2,
        Shape::Scissors => 3,
    };
}

fn score_outcome(opponent_move: Shape, my_move: Shape) -> i32 {
    if opponent_move == my_move  {
        return 3;
    } else if get_winning_shape(opponent_move) == my_move {
        return 6;
    } else {
        return 0;
    }
}

fn determine_my_move(opponent_move: Shape, desired_outcome: Outcome) -> Shape {
    return match desired_outcome {
        Outcome::Lose => {
            return get_losing_shape(opponent_move);
        },
        Outcome::Draw => opponent_move,
        Outcome::Win => {
            return get_winning_shape(opponent_move);
        },
    };
}

fn main() {
    let input = std::fs::read_to_string("input.txt").expect("could not read input");

    let mut total_score = 0;
    let mut total_ideal_score = 0;
    for line in input.lines() {
        let moves = line.split(' ').collect::<Vec<&str>>();

        let opponent_move = parse_shape(moves[0].chars().nth(0).unwrap());
        let my_move = parse_shape(moves[1].chars().nth(0).unwrap());
        let round_score = score_move(my_move) + score_outcome(opponent_move, my_move);
        total_score += round_score;

        let desired_outcome = parse_outcome(moves[1].chars().nth(0).unwrap());
        let desired_move = determine_my_move(opponent_move, desired_outcome);
        let ideal_round_score = score_move(desired_move) + score_outcome(opponent_move, desired_move);
        total_ideal_score += ideal_round_score;
    }

    println!("{total_score} {total_ideal_score}");
}

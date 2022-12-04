use itertools::Itertools;

fn find_common_char(first_str: &str, other_strs: &[&str]) -> char {
    for c in first_str.chars() {
        if other_strs.iter().map(|other_str| other_str.contains(c)).reduce(|a, b| a && b).unwrap() {
            return c;
        }
    }

    panic!("no matching character found");
}

fn score_char(c: char) -> u32 {
    if c >= 'a' && c <= 'z' {
        return (c as u32 - 'a' as u32) + 1;
    } else if c >= 'A' && c <= 'Z' {
        return (c as u32 - 'A' as u32) + 27;
    }

    panic!("invalid character");
}

fn main() {
    let input = std::fs::read_to_string("input.txt").expect("could not read input");

    let mut sum: u32 = 0;
    for line in input.lines() {
        let second_compartment_index = line.len() / 2;
        let first_compartment = &line[..second_compartment_index];
        let second_compartment = &line[second_compartment_index..];

        let common_char = find_common_char(first_compartment, &[second_compartment]);
        let char_score = score_char(common_char);
        sum += char_score;
    }
    println!("{sum}");

    let mut badge_sum: u32 = 0;
    for (first_rucksack, second_rucksack, third_rucksack) in input.lines().tuples() {
        let common_char = find_common_char(first_rucksack, &[second_rucksack, third_rucksack]);
        let char_score = score_char(common_char);
        badge_sum += char_score;
    }
    println!("{badge_sum}");
}

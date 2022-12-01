fn main() {
    let content = std::fs::read_to_string("input.txt").expect("could not read input");

    let mut calorie_counts = Vec::<i64>::new();
    let mut sum: i64 = 0;
    for line in content.lines() {
        match line.parse::<i64>() {
            Ok(n) => sum += n,
            Err(_) => {
                calorie_counts.push(sum);
                sum = 0;
            },
        }
    }
    println!("{}", calorie_counts.iter().max().unwrap());

    calorie_counts.sort();
    println!("{}", calorie_counts[calorie_counts.len() - 3 ..].iter().sum::<i64>());
}

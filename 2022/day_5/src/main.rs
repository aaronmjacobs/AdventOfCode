use sscanf::sscanf;

const DO_STACK_STUFF: bool = false;

fn main() {
    let input = std::fs::read_to_string("input.txt").expect("could not read input");

    let mut cargo = Vec::<Vec::<char>>::new();
    let mut parsing_stack = true;
    for line in input.lines() {
        if parsing_stack && (line.is_empty() || !line.contains('[')) {
            parsing_stack = false;
            for stack in &mut cargo {
                stack.reverse();
            }
            continue;
        }
        if line.is_empty() {
            continue;
        }

        if parsing_stack {
            let num_stacks = (line.chars().count() + 1) / 4;
            if cargo.len() < num_stacks {
                cargo.resize(num_stacks, Vec::<char>::new());
            }

            for stack_num in 0..num_stacks {
                let letter = line.chars().nth((stack_num * 4) + 1).unwrap();
                if letter.is_alphabetic() {
                    cargo[stack_num].push(letter);
                }
            }
        } else {
            let (quantity, from, to) = sscanf!(line, "move {} from {} to {}", usize, usize, usize).unwrap();

            if DO_STACK_STUFF {
                for _step in 0..quantity {
                    let letter = cargo[from - 1].pop().unwrap();
                    cargo[to - 1].push(letter);
                }
            } else {
                let mut temp_stack = Vec::<char>::new();
                for _step in 0..quantity {
                    let letter = cargo[from - 1].pop().unwrap();
                    temp_stack.push(letter);
                }

                for letter in temp_stack.iter().rev() {
                    cargo[to - 1].push(*letter);
                }
            }
        }
    }

    let mut message = String::new();
    for stack in &cargo {
        message.push(*stack.last().unwrap());
    }

    println!("{message}");
}

fn has_duplicate(arr: &[char]) -> bool {
    let mut itr: usize = 0;
    while itr < arr.len() - 1 {
        let mut cmp = itr + 1;
        while cmp < arr.len() {
            if arr[cmp] == arr[itr] {
                return true;
            }
            cmp += 1;
        }
        itr += 1;
    }

    false
}

fn find_marker_index(input: &String, num_unique_chars: usize) -> usize {
    let mut index: usize = 0;
    let mut buffer = vec!['\0'; num_unique_chars];

    for c in input.chars() {
        buffer[index % num_unique_chars] = c;
        index += 1;

        if index >= num_unique_chars && !has_duplicate(&buffer) {
            return index;
        }
    }

    panic!("invalid message");
}

fn main() {
    let input = std::fs::read_to_string("input.txt").expect("could not read input");

    let start_of_packet_index = find_marker_index(&input, 4);
    let start_of_message_index = find_marker_index(&input, 14);

    println!("{start_of_packet_index} {start_of_message_index}");
}
